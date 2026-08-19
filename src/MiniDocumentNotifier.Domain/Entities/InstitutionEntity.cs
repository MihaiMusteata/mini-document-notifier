using System.Collections.Generic;

namespace MiniDocumentNotifier.Domain.Entities
{
    public class InstitutionEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        
        // public ICollection<UserEntity> Users { get; set; }
        // public ICollection<DocumentEntity> Documents { get; set; }
    }
}
