using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.InstitutionContracts
{
    [DataContract]
    public class InstitutionDto
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
    }
}