using System.Collections.Generic;
using MiniDocumentNotifier.Domain.Entities;

namespace MiniDocumentNotifier.Domain.Repositories
{
    public interface IDocumentRepository
    {
        List<DocumentEntity> GetByInstitution(int institutionId);
    }
}