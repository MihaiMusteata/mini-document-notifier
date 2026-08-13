using System.Collections.Generic;
using System.ServiceModel;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Contracts.InstitutionContracts;

namespace MiniDocumentNotifier.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IDocumentNotifierService
    {
        [OperationContract]
        [FaultContract(typeof(AuthFault))]
        LoginResult Login(LoginRequest request);

        [OperationContract]
        List<InstitutionDto> GetInstitutions();
        
        [OperationContract]
        List<DocumentDto> GetDocuments(int institutionId);
        
    }
}