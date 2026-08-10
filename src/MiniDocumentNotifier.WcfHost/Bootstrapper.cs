using MiniDocumentNotifier.Application.Auth;
using MiniDocumentNotifier.Application.Institution;
using MiniDocumentNotifier.Domain.Repositories;
using MiniDocumentNotifier.Infrastructure.Security;
using MiniDocumentNotifier.Persistence.Repositories;
using Unity;

namespace MiniDocumentNotifier.WcfHost
{
    public static class Bootstrapper
    {
        public static readonly IUnityContainer Container = BuildContainer();
        
        private static IUnityContainer BuildContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IInstitutionRepository, InstitutionRepository>();
            container.RegisterType<IPasswordHasher, Pbkdf2PasswordHasher>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IInstitutionQueryService, IInstitutionQueryService>();

            return container;
        }
    }
}