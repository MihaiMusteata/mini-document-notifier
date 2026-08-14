using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.DocumentContracts
{
    [DataContract]
    public class DocumentQueryResult
    {
        [DataMember] public List<DocumentDto> Documents { get; set; }
        [DataMember] public int Total { get; set; }
    }
}