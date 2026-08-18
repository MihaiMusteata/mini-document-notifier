using System;

namespace MiniDocumentNotifier.Domain.Abstractions
{
    public interface ILogger
    {  
        void Info(string message);
        void Warning(string message);
        void Error(string message, Exception ex = null);
    }
}