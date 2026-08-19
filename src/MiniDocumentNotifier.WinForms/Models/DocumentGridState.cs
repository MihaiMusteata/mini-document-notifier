using MiniDocumentNotifier.Domain.Enums;

namespace MiniDocumentNotifier.WinForms.Models
{
    public class DocumentGridState
    {
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public DocumentType? TypeFilter { get; set; }
        public DocumentStatus? StatusFilter { get; set; }
    }
}