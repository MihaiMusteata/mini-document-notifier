using System.Security.Authentication;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;

namespace MiniDocumentNotifier.Application.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger _logger;

        public AuthenticationService(IPasswordHasher passwordHasher, IUserRepository userRepository, ILogger logger)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _logger = logger;
        }

        public UserEntity Authenticate(LoginRequest request)
        {
            var user = _userRepository.GetByUsernameAndInstitutionId(request.Username, request.InstitutionId);

            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                _logger.Warning($"Login failed: invalid credentials for user '{request.Username}' at institution {request.InstitutionId}.");
                throw new AuthenticationException("Username or password is incorrect");
            }

            if (!user.IsEnabled)
            {
                _logger.Warning($"Login failed: user '{request.Username}' is disabled.");
                throw new AuthenticationException("User is disabled");
            }

            _logger.Info($"Login succeeded for user '{request.Username}' at institution {request.InstitutionId}.");
            return user;
        }
    }
}