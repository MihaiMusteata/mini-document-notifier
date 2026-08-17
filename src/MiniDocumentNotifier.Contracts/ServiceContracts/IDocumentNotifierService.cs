using System.Collections.Generic;
using System.ServiceModel;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Contracts.DocumentUploadContracts;
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
        [FaultContract(typeof(InstitutionFault))]
        List<InstitutionDto> GetInstitutions();

        [OperationContract]
        [FaultContract(typeof(DocumentFault))]
        List<DocumentDto> GetDocuments(int institutionId);

        [OperationContract]
        [FaultContract(typeof(DocumentFault))]
        DocumentQueryResult GetDocumentsPaged(DocumentQueryRequest request);
        
        [OperationContract]
        [FaultContract(typeof(DocumentUploadFault))]
        DocumentUploadResult UploadDocument(DocumentUploadRequest request);
    }
}