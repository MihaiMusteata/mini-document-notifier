using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.AuthContracts
{
    [DataContract]
    public class AuthFault
    {
        [DataMember] public string Message { get; set; }
    }
}