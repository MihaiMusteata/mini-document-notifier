using MiniDocumentNotifier.BackgroundApp.UnityBootstrapper;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Infrastructure.Concurrency;
using Unity;

namespace MiniDocumentNotifier.BackgroundApp
{
    internal static class Program
    {
        private static void Main()
        {
            var logger = Bootstrapper.Container.Resolve<ILogger>();

            using (var mutexGuard = Bootstrapper.Container.Resolve<ISingleInstanceGuard>())
            {
                if (!mutexGuard.TryAcquire())
                {
                    logger.Warning("Background App startup blocked: another instance is already running.");
                    return;
                }

                logger.Info("Background App started: single instance acquired.");

                using (var signal = Bootstrapper.Container.Resolve<IBackgroundAppSignal>())
                {
                    signal.MarkActive();
                    Bootstrapper.Container.Resolve<SyncWorker>().Run();
                }
            }
        }
    }
}