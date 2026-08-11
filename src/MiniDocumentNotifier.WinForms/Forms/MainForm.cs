using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Infrastructure.ViewConfiguration;
using MiniDocumentNotifier.WinForms.Services;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly bool _isBackgroundAppRunning;
        private readonly int _institutionId;
        private InstitutionViewConfiguration _institutionConfiguration;

        public MainForm(bool isBackgroundAppRunning, int institutionId)
        {
            InitializeComponent();
            _isBackgroundAppRunning = isBackgroundAppRunning;
            _institutionId = institutionId;
            CheckConfiguration();
            Load += async (s, e) => await LoadDocumentsAsync();
        }

        private void CheckConfiguration()
        {
            var stalenessThresholdHours =
                int.Parse(ConfigurationManager.AppSettings["ViewConfigStalenessThresholdHours"]);
            var viewConfigPath = ConfigurationManager.AppSettings["ViewConfigPath"];
            var store = new JsonViewConfigurationStore(TimeSpan.FromHours(stalenessThresholdHours), viewConfigPath);

            var result = store.Load();

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

                documentsDataGrid.DataSource = filtered.Select(ToDisplayRow).ToList();
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(this, "Service is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(this, "Communication error with the service.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static object ToDisplayRow(DocumentDto document)
        {
            return new
            {
                document.Id,
                document.Name,
                Type = DocumentLabels.DocumentTypeLabels[document.Type],
                Status = DocumentLabels.DocumentStatusLabels[document.Status],
                document.UploadDate
            };
        }

    }
}