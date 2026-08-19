using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;

namespace MiniDocumentNotifier.WcfHost
{
    public class DocumentNotifierService : IDocumentNotifierService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IInstitutionQueryService _institutionQueryService;
        private readonly IDocumentQueryService _documentQueryService;
        private readonly IDocumentUploadService _documentUploadService;
        private readonly ILogger _logger;

        public DocumentNotifierService(IAuthenticationService authenticationService,
            IInstitutionQueryService institutionQueryService, IDocumentQueryService documentQueryService,
            IDocumentUploadService documentUploadService, ILogger logger)
        {
            _authenticationService = authenticationService;
            _institutionQueryService = institutionQueryService;
            _documentQueryService = documentQueryService;
            _documentUploadService = documentUploadService;
            _logger = logger;
            logger.Info("Document Notifier Service started");
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
                throw new FaultException<AuthFault>(new AuthFault { Message = ex.Message }, new FaultReason("Login rejected."));
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error during login for user '{request.Username}'.", ex);
                throw new FaultException<AuthFault>(new AuthFault { Message = "Login failed due to a server error." }, new FaultReason("Login error."));
            }
        }

        public List<InstitutionDto> GetInstitutions()
        {
                _logger.Info("Get institutions started.");
            try
            {
                var institutions = _institutionQueryService.GetAll();
                return institutions;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to load institutions.", ex);
                throw new FaultException<InstitutionFault>(
                    new InstitutionFault { Message = "Institutions could not be loaded." }, new FaultReason("Institution query failed."));
            }
        }

        public List<DocumentDto> GetDocuments(int institutionId)
        {
            try
            {
                var documents = _documentQueryService.GetByInstitution(institutionId);
                return documents;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to load documents for institution {institutionId}.", ex);
                throw new FaultException<DocumentFault>(
                    new DocumentFault { Message = "Documents could not be loaded." }, new FaultReason("Document query failed."));
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

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var result = _documentQueryService.GetPaged(query);
                stopwatch.Stop();

                _logger.Info($"Document query succeeded for institution {request.InstitutionId}: {result.Items.Count} of {result.TotalItems} document(s) returned in {stopwatch.ElapsedMilliseconds}ms.");

                return new DocumentQueryResult
                {
                    Total = result.TotalItems,
                    Documents = result.Items
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Document query failed for institution {request.InstitutionId}.", ex);
                throw new FaultException<DocumentFault>(
                    new DocumentFault { Message = "Document search failed. Try adjusting the filters." }, new FaultReason("Document query failed."));
            }
        }

        public DocumentUploadResult UploadDocument(DocumentUploadRequest request)
        {
            try
            {
                var documentId = _documentUploadService.Upload(request);

                _logger.Info($"Document uploaded: '{request.FileName}' ({request.Content?.Length ?? 0} bytes) for institution {request.InstitutionId}, new document id {documentId}.");

                return new DocumentUploadResult
                {
                    DocumentId = documentId,
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"Document upload failed: '{request.FileName}' for institution {request.InstitutionId}.", ex);
                throw new FaultException<DocumentUploadFault>(
                    new DocumentUploadFault { Message = "Document upload failed." }, new FaultReason("Upload error."));
            }
        }
    }
}