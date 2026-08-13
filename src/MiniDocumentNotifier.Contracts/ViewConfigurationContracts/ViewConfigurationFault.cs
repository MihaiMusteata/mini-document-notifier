using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.ViewConfigurationContracts
{
    [DataContract]
    public class ViewConfigurationFault
    {
        [DataMember] public string Message { get; set; }
    }
}