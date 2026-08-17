using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.DocumentUploadContracts
{
    [DataContract]
    public class DocumentUploadResult
    {
        [DataMember] public int DocumentId { get; set; }
    }
}