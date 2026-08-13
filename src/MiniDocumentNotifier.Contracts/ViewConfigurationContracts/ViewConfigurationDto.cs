using System;
using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.ViewConfigurationContracts
{
    [DataContract]
    public class ViewConfigurationDto
    {
        [DataMember] public int InstitutionId { get; set; }
        [DataMember] public string InstitutionCode { get; set; }
        [DataMember] public string VisibleColumns { get; set; }
        [DataMember] public string ActiveCategories { get; set; }
        [DataMember] public DateTime LastUpdatedDate { get; set; }
    }
}