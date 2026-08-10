using System.Security.Authentication;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;
using MiniDocumentNotifier.Infrastructure.Security;

namespace MiniDocumentNotifier.Application.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(IPasswordHasher passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public UserEntity Authenticate(LoginRequest request)
        {
            var user = _userRepository.GetByUsernameAndInstitutionId(request.Username, request.InstitutionId);

            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new AuthenticationException("Username or password is incorrect");

            return !user.IsEnabled ? throw new  AuthenticationException("User is disabled") : user;
        }
    }
}