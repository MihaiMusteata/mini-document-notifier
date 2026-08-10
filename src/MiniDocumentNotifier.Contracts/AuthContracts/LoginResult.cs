using System.Runtime.Serialization;

namespace MiniDocumentNotifier.Contracts.AuthContracts
{
    [DataContract]
    public class LoginResult
    {
        [DataMember] public int UserId { get; set; }
        [DataMember] public string Username { get; set; }
        [DataMember] public int  InstitutionId { get; set; }
        [DataMember] public string InstitutionName { get; set; }
    }
}