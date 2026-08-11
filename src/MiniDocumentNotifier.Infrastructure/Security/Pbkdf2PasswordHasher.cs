using System;
using System.Security.Cryptography;
using MiniDocumentNotifier.Domain.Abstractions;

namespace MiniDocumentNotifier.Infrastructure.Security
{
    public class Pbkdf2PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;

        public void Hash(string password, out string hash, out string salt)
        {
            var saltBytes = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256))
            {
                hash = Convert.ToBase64String(pbkdf2.GetBytes(KeySize));
            }

            salt = Convert.ToBase64String(saltBytes);
        }

        public bool Verify(string password, string hash, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(KeySize)) == hash;
            }
        }
    }
}