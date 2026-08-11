using System;
using System.Configuration;
using System.Threading;
using MiniDocumentNotifier.Application.Sync;
using MiniDocumentNotifier.Persistence.Repositories;

namespace MiniDocumentNotifier.BackgroundApp
{
    public static class SyncWorker
    {
        public static void Run()
        {
            var intervalSeconds = int.Parse(ConfigurationManager.AppSettings["IntervalSeconds"]);
            var outputFilePath = ConfigurationManager.AppSettings["OutputFilePath"];

            var syncService = new ViewConfigurationSyncService(
                new ViewConfigurationRepository(),
                outputFilePath);

            while (true)
            {
                try
                {
                    syncService.SyncAll();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
                
                Thread.Sleep(TimeSpan.FromSeconds(intervalSeconds));
            }
        }
    }
}