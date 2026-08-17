using System;
using System.Collections.Generic;
using System.IO;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.WpfControls.Commands;

namespace MiniDocumentNotifier.WpfControls.ViewModels
{
    public class DocumentUploadViewModel : ViewModelBase
    {
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
        public event Action UploadRequested;

        public DocumentUploadViewModel()
        {
            BrowseCommand = new RelayCommand(() => FileSelectionRequested?.Invoke());
            UploadCommand = new RelayCommand(
                () => UploadRequested?.Invoke(),
                () => !string.IsNullOrEmpty(FilePath) && !IsUploading);
        }
    }
}