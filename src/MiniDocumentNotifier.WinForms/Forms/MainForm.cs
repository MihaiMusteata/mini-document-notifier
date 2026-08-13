using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.WinForms.Services;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly bool _isBackgroundAppRunning;
        private readonly int _institutionId;
        private readonly IViewConfigurationStore _viewConfigurationStore;
        private InstitutionViewConfiguration _institutionConfiguration;
        private readonly UserPreferences _preferences;
        private List<DocumentRow> _rows = new List<DocumentRow>();
        private bool _suppressWidthCapture;

        public MainForm(IUserPreferencesStore preferencesStore, IViewConfigurationStore viewConfigurationStore, bool isBackgroundAppRunning, int institutionId)
        {
            InitializeComponent();
            _isBackgroundAppRunning = isBackgroundAppRunning;
            _institutionId = institutionId;
            _viewConfigurationStore = viewConfigurationStore;
            _preferences = preferencesStore.Load();

            CheckConfiguration();
            Load += async (s, e) => await LoadDocumentsAsync();
            FormClosing += (s, e) => preferencesStore.Save(_preferences);
        }

        private void CheckConfiguration()
        {
            var result = _viewConfigurationStore.Load();

            if (result.FileExists && !result.IsStale && _isBackgroundAppRunning)
            {
                _institutionConfiguration = result.Institutions?.FirstOrDefault(i => i.InstitutionId == _institutionId);
                return;
            }

            var message = !result.FileExists ? "Configuration file not found." :
                result.IsStale ? "Configuration file is stale." : "";

            if (!_isBackgroundAppRunning)
                message += " Background App is not running.";

            MessageBox.Show(this, message, "Configuration Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private async Task LoadDocumentsAsync()
        {
            try
            {
                var documents = await Task.Run(() =>
                {
                    using (var client = new DocumentNotifierServiceClient())
                    {
                        return client.GetDocuments(_institutionId);
                    }
                });

                var activeTypes = _institutionConfiguration != null
                    ? Newtonsoft.Json.JsonConvert.DeserializeObject<HashSet<DocumentType>>(_institutionConfiguration.ActiveCategories)
                    : null;

                var filtered = activeTypes == null
                    ? documents
                    : documents.Where(d => activeTypes.Contains(d.Type)).ToList();

                _rows = filtered.Select(ToRow).ToList();
                ApplySort(_preferences.DefaultSortColumn, _preferences.DefaultSortDescending);

                _suppressWidthCapture = true;
                BindGrid();
                ApplyColumnWidths();
                _suppressWidthCapture = false;
            }
            catch (FaultException<DocumentFault> fault)
            {
                MessageBox.Show(this, fault.Detail.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(this, "Service is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(this, "Communication error with the service.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (TimeoutException)
            {
                MessageBox.Show(this, "The service did not respond in time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static DocumentRow ToRow(DocumentDto document)
        {
            return new DocumentRow
            {
                Id = document.Id,
                Name = document.Name,
                Type = DocumentLabels.DocumentTypeLabels[document.Type],
                Status = DocumentLabels.DocumentStatusLabels[document.Status],
                UploadDate = document.UploadDate
            };
        }

        private void BindGrid()
        {
            documentsDataGrid.DataSource = null;
            documentsDataGrid.DataSource = _rows;
        }

        private void ApplySort(string columnName, bool descending)
        {
            if (string.IsNullOrEmpty(columnName)) return;

            var property = typeof(DocumentRow).GetProperty(columnName);
            if (property == null) return;

            _rows = descending
                ? _rows.OrderByDescending(property.GetValue).ToList()
                : _rows.OrderBy(property.GetValue).ToList();
        }

        private void ApplyColumnWidths()
        {
            foreach (DataGridViewColumn column in documentsDataGrid.Columns)
            {
                if (_preferences.ColumnWidths.TryGetValue(column.Name, out var weight))
                    column.FillWeight = weight;
            }
        }

        private void documentsDataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var columnName = documentsDataGrid.Columns[e.ColumnIndex].Name;
            var descending = _preferences.DefaultSortColumn == columnName && !_preferences.DefaultSortDescending;

            _preferences.DefaultSortColumn = columnName;
            _preferences.DefaultSortDescending = descending;

            ApplySort(columnName, descending);

            _suppressWidthCapture = true;
            BindGrid();
            ApplyColumnWidths();
            _suppressWidthCapture = false;
        }

        private void documentsDataGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (_suppressWidthCapture) return;

            _preferences.ColumnWidths[e.Column.Name] = e.Column.FillWeight;
        }
    }
}