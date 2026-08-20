using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Contracts.DocumentUploadContracts;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Infrastructure.ServiceClient;
using MiniDocumentNotifier.WpfControls.Commands;

namespace MiniDocumentNotifier.WpfControls.ViewModels
{
    public class DocumentUploadViewModel : ViewModelBase
    {
        private readonly IDocumentNotifierServiceClient _client;
        private readonly IFileStorage _fileStorage;
        private readonly int _institutionId;

        private const int MaxUploadSize = 5 * 1024 * 1024;

        private string _filePath;
        private string _documentName;
        private string _statusMessage;
        private bool _isUploading;
        private DocumentType _selectedType;

        public string FilePath
        {
            get => _filePath;
            set
            {
                SetField(ref _filePath, value);
                DocumentName = string.IsNullOrEmpty(value) ? string.Empty : Path.GetFileName(value);
            }
        }

        public string DocumentName
        {
            get => _documentName;
            set => SetField(ref _documentName, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        public bool IsUploading
        {
            get => _isUploading;
            set => SetField(ref _isUploading, value);
        }

        public DocumentType SelectedType
        {
            get => _selectedType;
            set => SetField(ref _selectedType, value);
        }

        public List<DocumentType> AvailableTypes { get; set; }


        public RelayCommand BrowseCommand { get; }
        public RelayCommand UploadCommand { get; }

        public event Action FileSelectionRequested;
        public event Action UploadCompleted;

        public DocumentUploadViewModel(
            IDocumentNotifierServiceClient client,
            IFileStorage fileStorage,
            int institutionId)
        {
            _client = client;
            _fileStorage = fileStorage;
            _institutionId = institutionId;

            AvailableTypes = Enum.GetValues(typeof(DocumentType)).Cast<DocumentType>().ToList();

            BrowseCommand = new RelayCommand(() => FileSelectionRequested?.Invoke());
            UploadCommand = new RelayCommand(
                async () => await UploadAsync(),
                () => !string.IsNullOrEmpty(FilePath) && !IsUploading);
        }

        public async Task UploadAsync()
        {
            IsUploading = true;
            StatusMessage = "Uploading...";

            try
            {
                var fileInfo = _fileStorage.GetInfo(FilePath);

                if (fileInfo.Length > MaxUploadSize)
                    throw new InvalidOperationException("The file is too large. Maximum size is 5MB");

                var request = new DocumentUploadRequest
                {
                    InstitutionId = _institutionId,
                    FileName = Path.GetFileName(FilePath),
                    Type = SelectedType,
                    Content = _fileStorage.ReadAllBytes(FilePath)
                };

                await Task.Run(() => _client.UploadDocument(request));

                StatusMessage = "Upload complete.";
                UploadCompleted?.Invoke();
            }
            catch (FaultException<DocumentUploadFault> fault)
            {
                StatusMessage = $"Upload failed: {fault.Detail.Message}";
            }
            catch (CommunicationException)
            {
                StatusMessage = "Communication error with the service.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Upload failed: {ex.Message}";
            }
            finally
            {
                IsUploading = false;
            }
        }
    }
}