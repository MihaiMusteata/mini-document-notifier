using MiniDocumentNotifier.BackgroundApp.UnityBootstrapper;
using MiniDocumentNotifier.Infrastructure.Concurrency;
using Unity;

namespace MiniDocumentNotifier.BackgroundApp
{
    internal static class Program
    {
        private static void Main()
        {
            using (var mutexGuard = Bootstrapper.Container.Resolve<ISingleInstanceGuard>())
            {
                if (!mutexGuard.TryAcquire())
                    return;

                using (var signal = Bootstrapper.Container.Resolve<IBackgroundAppSignal>())
                {
                    signal.MarkActive();
                    Bootstrapper.Container.Resolve<SyncWorker>().Run();
                }
            }
        }
    }
}