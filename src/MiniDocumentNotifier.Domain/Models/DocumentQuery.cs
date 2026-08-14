using System.Collections.Generic;
using MiniDocumentNotifier.Domain.Enums;

namespace MiniDocumentNotifier.Domain.Models
{
    public class DocumentQuery
    {
        public int InstitutionId  { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<DocumentType> AllowedTypes  { get; set; }
        public DocumentType? TypeFilter { get; set; }
        public DocumentStatus? StatusFilter { get; set; }
        public string SortColumn { get; set; } = "UploadDate";
        public bool SortDirection { get; set; } = true; // 1 -> Desc, 0 -> Asc
    }
}