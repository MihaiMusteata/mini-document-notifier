using System.Collections.Generic;

namespace MiniDocumentNotifier.Domain.Models
{
    public class ViewConfigurationResult
    {
        public bool FileExists { get; set; }
        public bool IsStale { get; set; }
        public List<InstitutionViewConfiguration> Institutions { get; set; }
    }
}