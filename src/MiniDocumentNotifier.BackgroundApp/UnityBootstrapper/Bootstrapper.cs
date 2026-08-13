using System;
using System.Configuration;
using MiniDocumentNotifier.Application.Sync;
using MiniDocumentNotifier.BackgroundApp.Client;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace MiniDocumentNotifier.BackgroundApp.UnityBootstrapper
{
    public static class Bootstrapper
    {
        public static readonly IUnityContainer Container = BuildContainer();

        private static IUnityContainer BuildContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IViewConfigurationSyncServiceClient, ViewConfigurationSyncServiceClient>(
                new ContainerControlledLifetimeManager());

            var outputFilePath =
                Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["OutputFilePath"]);
            container.RegisterType<IViewConfigurationSyncService, ViewConfigurationSyncService>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(typeof(IViewConfigurationSyncServiceClient), outputFilePath));

            return container;
        }
    }
}