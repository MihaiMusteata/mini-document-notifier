using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Infrastructure.Security;

namespace MiniDocumentNotifier.Infrastructure.Tests.Security
{
    [TestClass]
    public class Pbkdf2PasswordHasherTest
    {
        private Fixture _fixture;
        private IPasswordHasher _passwordHasher;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _passwordHasher = new Pbkdf2PasswordHasher();
        }

        [TestMethod]
        public void Hash_WhenPasswordIsValid_ReturnsHashAndSalt()
        {
            var password = _fixture.Create<string>();
            
            _passwordHasher.Hash(password, out var hash, out var salt);

            Assert.IsFalse(string.IsNullOrEmpty(hash));
            Assert.IsFalse(string.IsNullOrEmpty(salt));
        }
        
        [TestMethod]
        public void Verify_WhenPasswordIsCorrect_ReturnsTrue()
        {
            var password = _fixture.Create<string>();

            _passwordHasher.Hash(password, out var hash, out var salt);

            var result = _passwordHasher.Verify(password, hash, salt);

            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void Verify_WhenPasswordIsIncorrect_ReturnsFalse()
        {
            var password = _fixture.Create<string>();
            var wrongPassword = _fixture.Create<string>();

            _passwordHasher.Hash(password, out var hash, out var salt);

            var result = _passwordHasher.Verify(wrongPassword, hash, salt);

            Assert.IsFalse(result);
        }
        
    }
}