using System.Collections.Generic;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Domain.Models;

namespace MiniDocumentNotifier.Application.Document
{
    public interface IDocumentQueryService
    {
        List<DocumentDto> GetByInstitution(int institutionId);
        PagedResult<DocumentDto> GetPaged(DocumentQuery query);
    }
}