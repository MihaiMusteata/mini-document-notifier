namespace MiniDocumentNotifier.Domain.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsEnabled { get; set; }
        
        public int InstitutionId { get; set; }
        public InstitutionEntity Institution { get; set; }
    }
}
