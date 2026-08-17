using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.DocumentUploadContracts
{
    [DataContract]
    public class DocumentUploadFault
    {
        [DataMember] public string Message { get; set; }
    }
}