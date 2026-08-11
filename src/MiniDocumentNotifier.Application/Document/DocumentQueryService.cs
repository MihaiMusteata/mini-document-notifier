using System.Collections.Generic;
using System.Linq;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;

namespace MiniDocumentNotifier.Application.Document
{
    public class DocumentQueryService : IDocumentQueryService
    {
        private readonly IDocumentRepository _repository;

        public DocumentQueryService(IDocumentRepository repository)
        {
            _repository = repository;
        }

        public List<DocumentDto> GetByInstitution(int institutionId)
        {
            return _repository.GetByInstitution(institutionId)
                .Select(entity => new DocumentDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Type = DocumentLabels.DocumentTypeLabels[entity.Type],
                    Status = DocumentLabels.DocumentStatusLabels[entity.Status],
                    UploadDate = entity.UploadDate
                }).ToList();
        }
    }
}