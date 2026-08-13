using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.ServiceModel;
using MiniDocumentNotifier.Application.Auth;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Application.Institution;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Contracts.InstitutionContracts;
using MiniDocumentNotifier.Contracts.ServiceContracts;
using MiniDocumentNotifier.Domain.Repositories;

namespace MiniDocumentNotifier.WcfHost
{
    public class DocumentNotifierService : IDocumentNotifierService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IInstitutionQueryService _institutionQueryService;
        private readonly IDocumentQueryService _documentQueryService;

        public DocumentNotifierService(IAuthenticationService authenticationService,
            IInstitutionQueryService institutionQueryService, IDocumentQueryService documentQueryService)
        {
            _authenticationService = authenticationService;
            _institutionQueryService = institutionQueryService;
            _documentQueryService = documentQueryService;
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
    }
}