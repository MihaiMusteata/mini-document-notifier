using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.InstitutionContracts
{
    [DataContract]
    public class InstitutionFault
    {
        [DataMember] public string Message { get; set; }
    }
}
