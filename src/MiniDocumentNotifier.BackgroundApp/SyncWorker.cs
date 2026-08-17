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
        private readonly int _maxBackoffSeconds;

        private int _consecutiveFailures;

        public SyncWorker(IViewConfigurationSyncService syncService, int intervalSeconds, int maxBackoffSeconds)
        {
            _syncService = syncService;
            _intervalSeconds = intervalSeconds;
            _maxBackoffSeconds = maxBackoffSeconds;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    _syncService.SyncAll();
                    _consecutiveFailures = 0;
                }
                catch (CommunicationException ex)
                {
                    Console.Error.WriteLine($"WCF Host communication error: {ex.Message}");
                    _consecutiveFailures++;
                }
                catch (TimeoutException ex)
                {
                    Console.Error.WriteLine($"WCF Host timeout: {ex.Message}");
                    _consecutiveFailures++;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: {ex.Message}");
                    _consecutiveFailures++;
                }

                var backoff = _intervalSeconds * Math.Pow(2,  _consecutiveFailures);
                Thread.Sleep(TimeSpan.FromSeconds((int)Math.Min(backoff, _maxBackoffSeconds)));
            }
        }
    }
}