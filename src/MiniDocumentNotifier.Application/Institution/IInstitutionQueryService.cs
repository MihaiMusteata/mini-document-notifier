using System.Collections.Generic;
using MiniDocumentNotifier.Contracts.InstitutionContracts;

namespace MiniDocumentNotifier.Application.Institution
{
    public interface IInstitutionQueryService
    {
        List<InstitutionDto> GetAll();
    }
}