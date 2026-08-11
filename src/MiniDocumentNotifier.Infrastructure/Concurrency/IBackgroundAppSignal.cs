using System;

namespace MiniDocumentNotifier.Infrastructure.Concurrency
{
    public interface IBackgroundAppSignal : IDisposable
    {
        void MarkActive();
        bool IsActive();
    }
}