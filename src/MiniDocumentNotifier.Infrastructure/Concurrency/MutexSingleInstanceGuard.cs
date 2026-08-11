using System;
using System.Threading;

namespace MiniDocumentNotifier.Infrastructure.Concurrency
{
    public class MutexSingleInstanceGuard : ISingleInstanceGuard
    {
        private readonly Mutex _mutex;
        private bool _acquired;

        public MutexSingleInstanceGuard(string mutexName)
        {
            _mutex = new Mutex(false, mutexName);
        }

        public void Dispose()
        {
            if(_acquired)
                _mutex.ReleaseMutex();
            
            _mutex.Dispose();
        }

        public bool TryAcquire()
        {
            _acquired = _mutex.WaitOne(TimeSpan.Zero);
            return _acquired;
        }
    }
}