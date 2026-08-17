using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using MiniDocumentNotifier.Contracts.DocumentUploadContracts;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.WinForms.Services;
using MiniDocumentNotifier.WpfControls.ViewModels;
using MiniDocumentNotifier.WpfControls.Views;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class DocumentUploadForm : Form
    {
        private readonly DocumentUploadViewModel _viewModel;
        private readonly int _institutionId;

        public DocumentUploadForm(int institutionId)
        {
            InitializeComponent();

            _institutionId = institutionId;
            _viewModel = new DocumentUploadViewModel
            {
                AvailableTypes = Enum.GetValues(typeof(DocumentType)).Cast<DocumentType>().ToList()
            };
            _viewModel.FileSelectionRequested += OnFileSelectionRequested;
            _viewModel.UploadRequested += async () => await OnUploadRequestedAsync();

            var elementHost = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = new DocumentUploadView { DataContext = _viewModel }
            };

            Controls.Add(elementHost);
        }

        private void OnFileSelectionRequested()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = @"Documents|*.pdf;*.doc;*.docx ";

                if (dialog.ShowDialog() == DialogResult.OK)
                    _viewModel.FilePath = dialog.FileName;
            }
        }

        private async Task OnUploadRequestedAsync()
        {
            _viewModel.IsUploading = true;
            _viewModel.StatusMessage = "Uploading...";
            try
            {
                var fileBytes = File.ReadAllBytes(_viewModel.FilePath);

                var request = new DocumentUploadRequest
                {
                    InstitutionId = _institutionId,
                    FileName = Path.GetFileName(_viewModel.FilePath),
                    Type = _viewModel.SelectedType,
                    Content = fileBytes
                };

                await Task.Run(() =>
                {
                    using (var client = new DocumentNotifierServiceClient())
                    {
                        return client.UploadDocument(request);
                    }
                });

                _viewModel.StatusMessage = "Upload complete.";
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (FaultException<DocumentUploadFault> fault)
            {
                _viewModel.StatusMessage = $"Upload failed: {fault.Detail.Message}";
            }
            catch (CommunicationException)
            {
                _viewModel.StatusMessage = "Communication error with the service.";
            }
            catch (Exception ex)
            {
                _viewModel.StatusMessage = $"Upload failed: {ex.Message}";
            }
            finally
            {
                _viewModel.IsUploading = false;
            }
        }
    }
}