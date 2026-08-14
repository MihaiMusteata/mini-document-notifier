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
    public class DocumentGridState
    {
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public DocumentType? TypeFilter { get; set; }
        public DocumentStatus? StatusFilter { get; set; }
    }

    public partial class MainForm : Form
    {
        private const int PageSize = 20;
        private const int SearchDebounceMilliseconds = 500;

        private readonly bool _isBackgroundAppRunning;
        private readonly int _institutionId;
        private readonly IViewConfigurationStore _viewConfigurationStore;
        private readonly UserPreferences _preferences;
        private readonly DocumentGridState _gridState = new DocumentGridState();

        private InstitutionViewConfiguration _institutionConfiguration;
        private List<DocumentRow> _rows = new List<DocumentRow>();
        private bool _suppressWidthCapture;

        public MainForm(IUserPreferencesStore preferencesStore, IViewConfigurationStore viewConfigurationStore,
            bool isBackgroundAppRunning, int institutionId)
        {
            InitializeComponent();
            _isBackgroundAppRunning = isBackgroundAppRunning;
            _institutionId = institutionId;
            _viewConfigurationStore = viewConfigurationStore;
            _preferences = preferencesStore.Load();

            searchDebounceTimer.Interval = SearchDebounceMilliseconds;

            CheckConfiguration();
            PopulateFilterOptions();

            Load += async (s, e) => await LoadPageAsync();
            FormClosing += (s, e) =>
            {
                preferencesStore.Save(_preferences);
                searchDebounceTimer.Dispose();
            };
        }

        private void CheckConfiguration()
        {
            var result = _viewConfigurationStore.Load();

            if (!result.FileExists || result.IsStale || !_isBackgroundAppRunning)
            {
                var message = !result.FileExists ? "Configuration file not found." :
                    result.IsStale ? "Configuration file is stale." : "";

                if (!_isBackgroundAppRunning)
                    message += " Background App is not running.";

                ShowWarning(message);
                return;
            }

            _institutionConfiguration = result.Institutions?.FirstOrDefault(i => i.InstitutionId == _institutionId);

            if (_institutionConfiguration == null)
            {
                ShowWarning("No view configuration found for this institution. Showing all documents.");
            }
        }

        private void ShowWarning(string message)
        {
            lblWarning.Text = message;
            warningPanel.Visible = true;
        }

        private void BindGrid()
        {
            documentsDataGrid.DataSource = null;
            documentsDataGrid.DataSource = _rows;
        }

        private void ApplyColumnWidths()
        {
            foreach (DataGridViewColumn column in documentsDataGrid.Columns)
            {
                if (_preferences.ColumnWidths.TryGetValue(column.Name, out var weight))
                    column.FillWeight = weight;
            }
        }

        private async Task LoadPageAsync()
        {
            try
            {
                var request = new DocumentQueryRequest
                {
                    InstitutionId = _institutionId,
                    PageNumber = _gridState.CurrentPage,
                    PageSize = PageSize,
                    AllowedTypes = GetAllowedTypes(),
                    TypeFilter = _gridState.TypeFilter,
                    StatusFilter = _gridState.StatusFilter,
                    SortColumn = _preferences.DefaultSortColumn,
                    SortDirection = _preferences.DefaultSortDescending
                };

                var result = await Task.Run(() =>
                {
                    using (var client = new DocumentNotifierServiceClient())
                    {
                        return client.GetDocumentsPaged(request);
                    }
                });

                _gridState.TotalCount = result.Total;

                var searchTerm = txtSearch.Text?.Trim();
                var documents = result.Documents.AsEnumerable();

                if (!string.IsNullOrEmpty(searchTerm))
                    documents =
                        documents.Where(d => d.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);

                _rows = documents.Select(d => new DocumentRow
                {
                    Id = d.Id,
                    Name = d.Name,
                    Type = DocumentLabels.DocumentTypeLabels[d.Type],
                    Status = DocumentLabels.DocumentStatusLabels[d.Status],
                    UploadDate = d.UploadDate
                }).ToList();

                _suppressWidthCapture = true;
                BindGrid();
                ApplyColumnWidths();
                _suppressWidthCapture = false;

                UpdatePagingLabel();
            }
            catch (EndpointNotFoundException)
            {
                ShowWarning("Service is not available.");
            }
            catch (CommunicationException)
            {
                ShowWarning("Communication error with the service.");
            }
            catch (TimeoutException)
            {
                MessageBox.Show(this, @"Service timeout.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Helpers

        private List<DocumentType> GetAllowedTypes()
        {
            if (_institutionConfiguration == null) return null;

            return Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<string>>(_institutionConfiguration.ActiveCategories)
                .Select(t => (DocumentType)Enum.Parse(typeof(DocumentType), t))
                .ToList();
        }

        private void UpdatePagingLabel()
        {
            var totalPages = (int)Math.Ceiling(_gridState.TotalCount / (double)PageSize);
            lblPageInfo.Text = $@"Page {_gridState.CurrentPage + 1} of {Math.Max(totalPages, 1)}";
            btnPrevPage.Enabled = _gridState.CurrentPage > 0;
            btnNextPage.Enabled = (_gridState.CurrentPage + 1) * PageSize < _gridState.TotalCount;
        }

        private void PopulateFilterOptions()
        {
            var allowedTypes = GetAllowedTypes() ?? Enum.GetValues(typeof(DocumentType)).Cast<DocumentType>().ToList();

            var typeOptions = new List<string> { "All" };
            typeOptions.AddRange(allowedTypes.Select(t => DocumentLabels.DocumentTypeLabels[t]));
            cmbTypeFilter.DataSource = typeOptions;

            var statusOptions = new List<string> { "All" };
            statusOptions.AddRange(Enum.GetValues(typeof(DocumentStatus)).Cast<DocumentStatus>()
                .Select(s => DocumentLabels.DocumentStatusLabels[s]));
            cmbStatusFilter.DataSource = statusOptions;
        }

        #endregion


        #region Events Binding

        private void btnDismissWarning_Click(object sender, EventArgs e)
        {
            warningPanel.Visible = false;
        }

        private async void documentsDataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var columnName = documentsDataGrid.Columns[e.ColumnIndex].Name;
            var descending = _preferences.DefaultSortColumn == columnName && !_preferences.DefaultSortDescending;

            _preferences.DefaultSortColumn = columnName;
            _preferences.DefaultSortDescending = descending;

            await LoadPageAsync();
        }

        private void documentsDataGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (_suppressWidthCapture) return;

            _preferences.ColumnWidths[e.Column.Name] = e.Column.FillWeight;
        }

        private async void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (_gridState.CurrentPage == 0) return;
            _gridState.CurrentPage--;
            await LoadPageAsync();
        }

        private async void btnNextPage_Click(object sender, EventArgs e)
        {
            _gridState.CurrentPage++;
            await LoadPageAsync();
        }

        private async void cmbTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = cmbTypeFilter.SelectedItem?.ToString();
            _gridState.TypeFilter = selected == "All" || selected == null
                ? (DocumentType?)null
                : DocumentLabels.DocumentTypeLabels.FirstOrDefault(kv => kv.Value == selected).Key;

            _gridState.CurrentPage = 0;
            await LoadPageAsync();
        }

        private async void cmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = cmbStatusFilter.SelectedItem?.ToString();
            _gridState.StatusFilter = selected == "All" || selected == null
                ? (DocumentStatus?)null
                : DocumentLabels.DocumentStatusLabels.FirstOrDefault(kv => kv.Value == selected).Key;

            _gridState.CurrentPage = 0;
            await LoadPageAsync();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchDebounceTimer.Stop();
            searchDebounceTimer.Start();
        }

        #endregion

        private async void searchDebounceTimer_Tick(object sender, EventArgs e)
        {
            searchDebounceTimer.Stop();
            _gridState.CurrentPage = 0;
            await LoadPageAsync();
        }
    }
}