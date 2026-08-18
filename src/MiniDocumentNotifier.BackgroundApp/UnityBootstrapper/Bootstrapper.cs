using System;
using System.Configuration;
using MiniDocumentNotifier.Application.Sync;
using MiniDocumentNotifier.BackgroundApp.Client;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Infrastructure.Concurrency;
using MiniDocumentNotifier.Infrastructure.Logging;
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
                new InjectionConstructor(typeof(IViewConfigurationSyncServiceClient), outputFilePath, typeof(ILogger)));

            container.RegisterType<ISingleInstanceGuard, MutexSingleInstanceGuard>(new TransientLifetimeManager(),
                new InjectionConstructor(Constants.BackgroundAppMutexName));
            container.RegisterType<IBackgroundAppSignal, SemaphoreBackgroundAppSignal>(new TransientLifetimeManager(),
                new InjectionConstructor(Constants.BackgroundAppSemaphoreName));

            var intervalSeconds = int.Parse(ConfigurationManager.AppSettings["IntervalSeconds"]);
            var maxBackoffSeconds = int.Parse(ConfigurationManager.AppSettings["MaxBackoffSeconds"]);
            container.RegisterType<SyncWorker>(new TransientLifetimeManager(),
                new InjectionConstructor(typeof(IViewConfigurationSyncService), intervalSeconds, maxBackoffSeconds,
                    typeof(ILogger)));

            container.RegisterType<ILogger, NLogLogger>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}