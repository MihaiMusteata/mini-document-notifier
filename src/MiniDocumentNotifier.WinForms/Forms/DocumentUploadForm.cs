using System.Windows.Forms;
using System.Windows.Forms.Integration;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Infrastructure.ServiceClient;
using MiniDocumentNotifier.WpfControls.ViewModels;
using MiniDocumentNotifier.WpfControls.Views;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class DocumentUploadForm : Form
    {
        private readonly DocumentUploadViewModel _viewModel;

        public DocumentUploadForm(
            int institutionId,
            IDocumentNotifierServiceClient client,
            IFileStorage fileReader)
        {
            InitializeComponent();

            _viewModel = new DocumentUploadViewModel(client, fileReader, institutionId);
            _viewModel.FileSelectionRequested += OnFileSelectionRequested;
            _viewModel.UploadCompleted += OnUploadCompleted;

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
                dialog.Filter = @"Documents|*.pdf;*.doc;*.docx";

                if (dialog.ShowDialog() == DialogResult.OK)
                    _viewModel.FilePath = dialog.FileName;
            }
        }

        private void OnUploadCompleted()
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}