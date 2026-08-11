using System;
using System.Threading;

namespace MiniDocumentNotifier.Infrastructure.Concurrency
{
    public class SemaphoreBackgroundAppSignal : IBackgroundAppSignal
    {
        private readonly Semaphore _semaphore;
        private bool _held;

        public SemaphoreBackgroundAppSignal(string semaphoreName)
        {
            _semaphore = new Semaphore(1, 1, semaphoreName);
        }

        public void Dispose()
        {
            if (_held)
            {
                _semaphore.Release();
            }

            _semaphore.Dispose();
        }

        public void MarkActive()
        {
            _held = _semaphore.WaitOne(TimeSpan.Zero);
        }

        public bool IsActive()
        {
            var acquired = _semaphore.WaitOne(TimeSpan.Zero);

            if (acquired)
            {
                _semaphore.Release();
            }

            return !acquired;
        }
    }
}