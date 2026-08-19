using System.Security.Authentication;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Auth;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;
using Moq;

namespace MiniDocumentNotifier.Application.Tests.Auth
{
    [TestClass]
    public class AuthenticationServiceTest
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IPasswordHasher> _passwordHasher;
        private Mock<ILogger> _logger;
        private Fixture _fixture;
        private IAuthenticationService _service;

        [TestInitialize]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _passwordHasher = new Mock<IPasswordHasher>();
            _logger = new Mock<ILogger>();
            _fixture = new Fixture();
            _service = new AuthenticationService(
                _passwordHasher.Object,
                _userRepository.Object,
                _logger.Object);
        }

        [TestMethod]
        public void Authenticate_WhenCredentialsAreValid_ReturnsUser()
        {
            var request = _fixture.Create<LoginRequest>();
            var user = _fixture
                .Build<UserEntity>()
                .With(x => x.IsEnabled, true)
                .Create();

            _userRepository
                .Setup(x => x.GetByUsernameAndInstitutionId(request.Username, request.InstitutionId))
                .Returns(user);

            _passwordHasher
                .Setup(x => x.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
                .Returns(true);

            var result = _service.Authenticate(request);

            Assert.AreSame(user, result);

            _userRepository.Verify(
                x => x.GetByUsernameAndInstitutionId(request.Username, request.InstitutionId),
                Times.Once);

            _passwordHasher.Verify(
                x => x.Verify(request.Password, user.PasswordHash, user.PasswordSalt),
                Times.Once);
        }

        [TestMethod]
        public void Authenticate_WhenUserDoesNotExist_ThrowsAuthenticationException()
        {
            var request = _fixture.Create<LoginRequest>();

            _userRepository
                .Setup(x => x.GetByUsernameAndInstitutionId(request.Username, request.InstitutionId))
                .Returns((UserEntity)null);

            Assert.ThrowsException<AuthenticationException>(() => _service.Authenticate(request));

            _passwordHasher.Verify(
                x => x.Verify(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [TestMethod]
        public void Authenticate_WhenPasswordIsWrong_ThrowsAuthenticationException()
        {
            var request = _fixture.Create<LoginRequest>();
            var user = _fixture
                .Build<UserEntity>()
                .With(x => x.IsEnabled, true)
                .Create();

            _userRepository
                .Setup(x => x.GetByUsernameAndInstitutionId(request.Username, request.InstitutionId))
                .Returns(user);

            _passwordHasher
                .Setup(x => x.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
                .Returns(false);


            Assert.ThrowsException<AuthenticationException>(() => _service.Authenticate(request));
        }

        [TestMethod]
        public void Authenticate_WhenUserIsDisabled_ThrowsAuthenticationException()
        {
            var request = _fixture.Create<LoginRequest>();
            var user = _fixture
                .Build<UserEntity>()
                .With(x => x.IsEnabled, false)
                .Create();

            _userRepository
                .Setup(x => x.GetByUsernameAndInstitutionId(request.Username, request.InstitutionId))
                .Returns(user);

            _passwordHasher
                .Setup(x => x.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
                .Returns(true);

            Assert.ThrowsException<AuthenticationException>(() => _service.Authenticate(request));
        }
    }
}