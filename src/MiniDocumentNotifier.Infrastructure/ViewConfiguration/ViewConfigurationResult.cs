using System.Collections.Generic;

namespace MiniDocumentNotifier.Infrastructure.ViewConfiguration
{
    public class ViewConfigurationResult
    {
        public bool FileExists { get; set; }
        public bool IsStale { get; set; }
        public List<InstitutionViewConfiguration> Institutions { get; set; }
    }

    public class InstitutionViewConfiguration
    {
        public int InstitutionId { get; set; }
        public string VisibleColumns { get; set; }
        public string ActiveCategories { get; set; }
    }
}