using System;
using NLog;

namespace MiniDocumentNotifier.Infrastructure.Logging
{
    public class NLogLogger :  MiniDocumentNotifier.Domain.Abstractions.ILogger
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string message, Exception ex = null)
        {
            _logger.Error(ex, message);
        }
    }
}