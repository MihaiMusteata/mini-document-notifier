using System.Runtime.Serialization;
using MiniDocumentNotifier.Domain.Enums;

namespace MiniDocumentNotifier.Contracts.DocumentUploadContracts
{
    [DataContract]
    public class DocumentUploadRequest
    {
        [DataMember] public int InstitutionId  { get; set; }
        [DataMember] public string FileName { get; set; }
        [DataMember] public DocumentType Type { get; set; }
        [DataMember] public byte[] Content { get; set; }
    }
}