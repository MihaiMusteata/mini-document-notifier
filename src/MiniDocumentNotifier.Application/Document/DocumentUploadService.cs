using System;
using System.IO;
using MiniDocumentNotifier.Contracts.DocumentUploadContracts;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Domain.Repositories;

namespace MiniDocumentNotifier.Application.Document
{
    public class DocumentUploadService : IDocumentUploadService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IFileStorage _fileStorage;
        private readonly string _storageRootPath;

        public DocumentUploadService(IDocumentRepository documentRepository, IFileStorage fileStorage,
            string storageRootPath)
        {
            _documentRepository = documentRepository;
            _storageRootPath = storageRootPath;
            _fileStorage = fileStorage;
        }

        public int Upload(DocumentUploadRequest documentUploadModel)
        {
            if (documentUploadModel.Content == null || documentUploadModel.Content.Length == 0)
                throw new ArgumentException("File content is empty.");

            var institutionFolder = Path.Combine(_storageRootPath, documentUploadModel.InstitutionId.ToString());
            _fileStorage.CreateDirectory(institutionFolder);

            var safeFileName = $"{Guid.NewGuid()}_{Path.GetFileName(documentUploadModel.FileName)}";
            var fullPath = Path.Combine(institutionFolder, safeFileName);

            _fileStorage.WriteAllBytes(fullPath, documentUploadModel.Content);

            var document = new DocumentEntity
            {
                InstitutionId = documentUploadModel.InstitutionId,
                Name = documentUploadModel.FileName,
                Type = documentUploadModel.Type,
                UploadDate = DateTime.UtcNow,
                Status = DocumentStatus.New
            };

            return _documentRepository.Insert(document);
        }
    }
}