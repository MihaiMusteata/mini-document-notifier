using System;
using System.ServiceModel;
using System.Threading;
using MiniDocumentNotifier.Application.Sync;

namespace MiniDocumentNotifier.BackgroundApp
{
    public class SyncWorker
    {
        private readonly IViewConfigurationSyncService _syncService;
        private readonly int _intervalSeconds;

        public SyncWorker(IViewConfigurationSyncService syncService, int intervalSeconds)
        {
            _syncService = syncService;
            _intervalSeconds = intervalSeconds;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    _syncService.SyncAll();
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

                Thread.Sleep(TimeSpan.FromSeconds(_intervalSeconds));
            }
        }
    }
}