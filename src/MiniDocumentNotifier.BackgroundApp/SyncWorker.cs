using System;
using System.Configuration;
using System.ServiceModel;
using System.Threading;
using MiniDocumentNotifier.Application.Sync;
using MiniDocumentNotifier.BackgroundApp.UnityBootstrapper;
using Unity;

namespace MiniDocumentNotifier.BackgroundApp
{
    public static class SyncWorker
    {
        public static void Run()
        {
            var intervalSeconds = int.Parse(ConfigurationManager.AppSettings["IntervalSeconds"]);
            var syncService = Bootstrapper.Container.Resolve<IViewConfigurationSyncService>();

            while (true)
            {
                try
                {
                    syncService.SyncAll();
                }
                catch (CommunicationException ex)
                {
                    Console.Error.WriteLine($"WCF Host communication error: {ex.Message}");
                }
                catch (TimeoutException ex)
                {
                    Console.Error.WriteLine($"WCF Host timeout: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: {ex.Message}");
                }

                Thread.Sleep(TimeSpan.FromSeconds(intervalSeconds));
            }
        }
    }
}