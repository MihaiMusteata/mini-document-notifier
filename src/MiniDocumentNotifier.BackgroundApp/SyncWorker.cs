using System;
using System.ServiceModel;
using System.Threading;
using MiniDocumentNotifier.Application.Sync;
using MiniDocumentNotifier.Domain.Abstractions;

namespace MiniDocumentNotifier.BackgroundApp
{
    public class SyncWorker
    {
        private readonly IViewConfigurationSyncService _syncService;
        private readonly int _intervalSeconds;
        private readonly int _maxBackoffSeconds;
        private readonly ILogger _logger;

        private int _consecutiveFailures;

        public SyncWorker(IViewConfigurationSyncService syncService, int intervalSeconds, int maxBackoffSeconds,
            ILogger logger)
        {
            _syncService = syncService;
            _intervalSeconds = intervalSeconds;
            _maxBackoffSeconds = maxBackoffSeconds;
            _logger = logger;
        }

        public void Run()
        {
            var random = new Random();

            while (true)
            {
                _logger.Info("View configuration sync cycle started.");

                try
                {
                    _syncService.SyncAll();

                    if (_consecutiveFailures > 0)
                    {
                        _logger.Info($"View configuration sync recovered after {_consecutiveFailures} consecutive failure(s); backoff reset.");
                    }

                    _consecutiveFailures = 0;
                }
                catch (CommunicationException ex)
                {
                    _consecutiveFailures++;
                    _logger.Error("View configuration sync failed: WCF Host unreachable or database inaccessible.", ex);
                }
                catch (TimeoutException ex)
                {
                    _consecutiveFailures++;
                    _logger.Error("View configuration sync failed: WCF Host timeout.", ex);
                }
                catch (Exception ex)
                {
                    _consecutiveFailures++;
                    _logger.Error("View configuration sync failed with an unexpected error.", ex);
                }

                var backoff = _intervalSeconds * Math.Pow(2, _consecutiveFailures);
                var delaySeconds = (int)Math.Min(backoff, _maxBackoffSeconds);

                if (_consecutiveFailures > 0)
                {
                    _logger.Warning($"Applying backoff after {_consecutiveFailures} consecutive failure(s); waiting {delaySeconds}s before next sync attempt.");
                }

                var jitter = random.NextDouble() * 0.2 - 0.1;
                Thread.Sleep(TimeSpan.FromSeconds(delaySeconds * (1 + jitter)));
            }
        }
    }
}