using System.Collections.Generic;
using MiniDocumentNotifier.Contracts.DocumentContracts;

namespace MiniDocumentNotifier.Application.Document
{
    public interface IDocumentQueryService
    {
        List<DocumentDto> GetByInstitution(int institutionId);
    }
}