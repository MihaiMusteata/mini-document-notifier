using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.ServiceModel;
using MiniDocumentNotifier.Application.Auth;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Application.Institution;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Contracts.DocumentUploadContracts;
using MiniDocumentNotifier.Contracts.InstitutionContracts;
using MiniDocumentNotifier.Contracts.ServiceContracts;
using MiniDocumentNotifier.Domain.Models;

namespace MiniDocumentNotifier.WcfHost
{
    public class DocumentNotifierService : IDocumentNotifierService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IInstitutionQueryService _institutionQueryService;
        private readonly IDocumentQueryService _documentQueryService;
        private readonly IDocumentUploadService _documentUploadService;

        public DocumentNotifierService(IAuthenticationService authenticationService,
            IInstitutionQueryService institutionQueryService, IDocumentQueryService documentQueryService,
            IDocumentUploadService documentUploadService)
        {
            _authenticationService = authenticationService;
            _institutionQueryService = institutionQueryService;
            _documentQueryService = documentQueryService;
            _documentUploadService = documentUploadService;
        }

        public LoginResult Login(LoginRequest request)
        {
            try
            {
                var user = _authenticationService.Authenticate(request);

                return new LoginResult
                {
                    UserId = user.Id,
                    Username = user.Username,
                    InstitutionId = user.InstitutionId
                };
            }
            catch (AuthenticationException ex)
            {
                throw new FaultException<AuthFault>(new AuthFault { Message = ex.Message }, new FaultReason("Failed."));
            }
        }

        public List<InstitutionDto> GetInstitutions()
        {
            try
            {
                var institutions = _institutionQueryService.GetAll();
                return institutions;
            }
            catch (Exception)
            {
                throw new FaultException<InstitutionFault>(
                    new InstitutionFault { Message = "Error getting institutions" }, new FaultReason("Failed."));
            }
        }

        public List<DocumentDto> GetDocuments(int institutionId)
        {
            try
            {
                var documents = _documentQueryService.GetByInstitution(institutionId);
                return documents;
            }
            catch (Exception)
            {
                throw new FaultException<DocumentFault>(
                    new DocumentFault { Message = "Error getting documents" }, new FaultReason("Failed."));
            }
        }

        public DocumentQueryResult GetDocumentsPaged(DocumentQueryRequest request)
        {
            var query = new DocumentQuery
            {
                InstitutionId = request.InstitutionId,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TypeFilter = request.TypeFilter,
                StatusFilter = request.StatusFilter,
                SortColumn = string.IsNullOrEmpty(request.SortColumn) ? "UploadDate" : request.SortColumn,
                SortDirection = request.SortDirection,
                AllowedTypes = request.AllowedTypes
            };

            try
            {
                var result = _documentQueryService.GetPaged(query);

                return new DocumentQueryResult
                {
                    Total = result.TotalItems,
                    Documents = result.Items
                };
            }
            catch (Exception)
            {
                throw new FaultException<DocumentFault>(
                    new DocumentFault { Message = "Error getting documents" }, new FaultReason("Failed."));
            }
        }

        public DocumentUploadResult UploadDocument(DocumentUploadRequest request)
        {
            try
            {
                var documentId = _documentUploadService.Upload(request);
                return new DocumentUploadResult
                {
                    DocumentId = documentId,
                };
            }
            catch (Exception ex)
            {
                throw new FaultException<DocumentUploadFault>(new DocumentUploadFault { Message = ex.Message });
            }
        }
    }
}