using System.Collections.Generic;
using System.Runtime.Serialization;
using MiniDocumentNotifier.Domain.Enums;

namespace MiniDocumentNotifier.Contracts.DocumentContracts
{
    [DataContract]
    public class DocumentQueryRequest
    {
        [DataMember] public int InstitutionId { get; set; }
        [DataMember] public int PageNumber { get; set; }
        [DataMember] public int PageSize { get; set; }
        [DataMember] public List<DocumentType> AllowedTypes { get; set; }
        [DataMember] public DocumentType? TypeFilter { get; set; }
        [DataMember] public DocumentStatus? StatusFilter { get; set; }
        [DataMember] public string SortColumn { get; set; }
        [DataMember] public bool SortDirection { get; set; }
    }
}