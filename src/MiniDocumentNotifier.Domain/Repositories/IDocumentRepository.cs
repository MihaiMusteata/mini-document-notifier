using System.Collections.Generic;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Models;

namespace MiniDocumentNotifier.Domain.Repositories
{
    public interface IDocumentRepository
    {
        List<DocumentEntity> GetByInstitution(int institutionId);
        PagedResult<DocumentEntity> GetPaged(DocumentQuery query);
        int Insert(DocumentEntity document);
    }
}