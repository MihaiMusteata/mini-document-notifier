using System.Configuration;
using MiniDocumentNotifier.Application.Auth;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Application.Institution;
using MiniDocumentNotifier.Application.ViewConfiguration;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Repositories;
using MiniDocumentNotifier.Infrastructure.Logging;
using MiniDocumentNotifier.Infrastructure.Security;
using MiniDocumentNotifier.Persistence.Repositories;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace MiniDocumentNotifier.WcfHost.UnityBootstrapper
{
    public static class Bootstrapper
    {
        public static readonly IUnityContainer Container = BuildContainer();

        private static IUnityContainer BuildContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IInstitutionRepository, InstitutionRepository>();
            container.RegisterType<IDocumentRepository, DocumentRepository>();
            container.RegisterType<IViewConfigurationRepository, ViewConfigurationRepository>();

            container.RegisterType<IPasswordHasher, Pbkdf2PasswordHasher>();
            container.RegisterType<IFileStorage, FileStorage>();

            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IInstitutionQueryService, InstitutionQueryService>();
            container.RegisterType<IDocumentQueryService, DocumentQueryService>();
            container.RegisterType<IViewConfigurationQueryService, ViewConfigurationQueryService>();
           
            container.RegisterType<IDocumentUploadService, DocumentUploadService>(
                new InjectionConstructor(
                    new ResolvedParameter<IDocumentRepository>(),
                    new ResolvedParameter<IFileStorage>(),
                    ConfigurationManager.AppSettings["DocumentStorageRootPath"]));

            container.RegisterType<ILogger, NLogLogger>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}