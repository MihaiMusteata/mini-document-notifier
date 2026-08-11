using System;

namespace MiniDocumentNotifier.Infrastructure.Concurrency
{
    public interface ISingleInstanceGuard : IDisposable
    {
        bool TryAcquire();
    }
}