using MiniDocumentNotifier.Domain.Entities;

namespace MiniDocumentNotifier.Domain.Repositories
{
    public interface IUserRepository
    {
        UserEntity GetByUsernameAndInstitutionId(string username, int institutionId);
        void Register(UserEntity user);
    }
}