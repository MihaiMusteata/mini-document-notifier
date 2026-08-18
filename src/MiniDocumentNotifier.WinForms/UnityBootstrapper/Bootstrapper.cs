using System;
using System.Configuration;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Infrastructure.Concurrency;
using MiniDocumentNotifier.Infrastructure.Logging;
using MiniDocumentNotifier.Infrastructure.Preferences;
using MiniDocumentNotifier.Infrastructure.ViewConfiguration;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace MiniDocumentNotifier.WinForms.UnityBootstrapper
{
    public static class Bootstrapper
    {
        public static readonly IUnityContainer Container = BuildContainer();

        private static IUnityContainer BuildContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<ILogger, NLogLogger>(new ContainerControlledLifetimeManager());
            var logger = container.Resolve<ILogger>();

            container.RegisterInstance(CreateUserPreferencesStore(logger), new SingletonLifetimeManager());

            var viewConfigPath = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["ViewConfigPath"]);
            var stalenessHours = int.Parse(ConfigurationManager.AppSettings["ViewConfigStalenessThresholdHours"]);
            container.RegisterInstance<IViewConfigurationStore>(new JsonViewConfigurationStore(TimeSpan.FromHours(stalenessHours), viewConfigPath, logger), new SingletonLifetimeManager());

            container.RegisterType<AppContext>(new TransientLifetimeManager());

            container.RegisterType<ISingleInstanceGuard, MutexSingleInstanceGuard>(new TransientLifetimeManager(),
                new InjectionConstructor(Constants.WinFormsMutexName));
            container.RegisterType<IBackgroundAppSignal, SemaphoreBackgroundAppSignal>(new TransientLifetimeManager(),
                new InjectionConstructor(Constants.BackgroundAppSemaphoreName));

            return container;

        }

        private static IUserPreferencesStore CreateUserPreferencesStore(ILogger logger)
        {
            var source = ConfigurationManager.AppSettings["UserPreferencesSource"];

            if (string.Equals(source, "Registry",StringComparison.CurrentCultureIgnoreCase))
            {
                var registryKey = ConfigurationManager.AppSettings["UserPreferencesRegistryKey"];
                return new RegistryUserPreferencesStore(registryKey, logger);
            }

            var jsonPath = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["UserPreferencesPath"]);
            return new  JsonUserPreferencesStore(jsonPath, logger);
        }
    }
}