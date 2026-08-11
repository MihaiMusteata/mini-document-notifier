
using MiniDocumentNotifier.Infrastructure.Concurrency;

namespace MiniDocumentNotifier.BackgroundApp
{
    internal static class Program
    {
        private static void Main()
        {
            using (var mutexGuard = new MutexSingleInstanceGuard(Constants.BackgroundAppMutexName))
            {
                if (!mutexGuard.TryAcquire())
                    return;

                using (var signal = new SemaphoreBackgroundAppSignal(Constants.BackgroundAppSemaphoreName))
                {
                    signal.MarkActive();
                    SyncWorker.Run();
                }
            }
        }
    }
}