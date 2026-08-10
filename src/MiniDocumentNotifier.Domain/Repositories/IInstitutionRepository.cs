using System.Collections.Generic;
using MiniDocumentNotifier.Domain.Entities;

namespace MiniDocumentNotifier.Domain.Repositories
{
    public interface IInstitutionRepository
    {
        List<InstitutionEntity> GetAll();
        InstitutionEntity GetById(int institutionId);
    }
}