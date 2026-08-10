using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.AuthContracts
{
    [DataContract]
    public class LoginRequest
    {
         [DataMember] public int InstitutionId { get; set; }
         [DataMember] public string Username { get; set; }
         [DataMember] public string Password { get; set; }
    }
}