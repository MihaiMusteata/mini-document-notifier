using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.DocumentContracts
{
    [DataContract]
    public class DocumentFault
    {
        [DataMember] public string Message { get; set; }
    }
}
