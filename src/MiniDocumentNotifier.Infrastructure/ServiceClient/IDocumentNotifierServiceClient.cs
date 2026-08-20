using System;
using System.Collections.Generic;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Contracts.DocumentUploadContracts;
using MiniDocumentNotifier.Contracts.InstitutionContracts;

namespace MiniDocumentNotifier.Infrastructure.ServiceClient
{
    public interface IDocumentNotifierServiceClient : IDisposable
    {
        LoginResult Login(LoginRequest request);
        List<InstitutionDto> GetInstitutions();
        List<DocumentDto> GetDocuments(int institutionId);
        DocumentQueryResult GetDocumentsPaged(DocumentQueryRequest request);
        DocumentUploadResult UploadDocument(DocumentUploadRequest request);
    }
}