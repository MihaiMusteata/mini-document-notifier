namespace MiniDocumentNotifier.Infrastructure.Security
{
    public interface IPasswordHasher
    {
        void Hash(string password, out string hash, out string salt);
        bool Verify(string password, string hash, string salt);
    }
}