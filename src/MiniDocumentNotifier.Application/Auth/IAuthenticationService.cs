using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Domain.Entities;

namespace MiniDocumentNotifier.Application.Auth
{
    public interface IAuthenticationService
    {
        UserEntity Authenticate(LoginRequest request);
    }
}