using System;
using MiniDocumentNotifier.Domain.Enums;

namespace MiniDocumentNotifier.Domain.Entities
{
    public class DocumentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DocumentType Type { get; set; }
        public DateTime UploadDate { get; set; }
        public DocumentStatus Status { get; set; }
        
        public int InstitutionId { get; set; }
        // public InstitutionEntity Institution { get; set; }
    }
}
