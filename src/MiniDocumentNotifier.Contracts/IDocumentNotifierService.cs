using System.Collections.Generic;
using System.ServiceModel;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Contracts.InstitutionContracts;

namespace MiniDocumentNotifier.Contracts
{
    [ServiceContract]
    public interface IDocumentNotifierService
    {
        [OperationContract]
        [FaultContract(typeof(AuthFault))]
        LoginResult Login(LoginRequest request);

        [OperationContract]
        List<InstitutionDto> GetInstitutions();
        
    }
}