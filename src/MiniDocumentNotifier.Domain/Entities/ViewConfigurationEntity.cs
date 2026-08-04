using System;

namespace MiniDocumentNotifier.Domain.Entities
{
    public class ViewConfigurationEntity
    {
        public int Id { get; set; }
        public string VisibleColumns { get; set; }
        public string ActiveCategories { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public int InstitutionId { get; set; }
        public InstitutionEntity Institution { get; set; }
    }
}
