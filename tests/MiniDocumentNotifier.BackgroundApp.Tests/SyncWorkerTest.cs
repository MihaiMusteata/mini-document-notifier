using System;
using System.ServiceModel;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Sync;
using MiniDocumentNotifier.Domain.Abstractions;
using Moq;

namespace MiniDocumentNotifier.BackgroundApp.Tests
{
    [TestClass]
    public class SyncWorkerTest
    {
        private Mock<IViewConfigurationSyncService> _syncService;
        private Mock<ILogger> _logger;
        private Fixture _fixture;

        private SyncWorker _worker;
        private const int IntervalSeconds = 10;
        private const int MaxBackoffSeconds = 300;

        [TestInitialize]
        public void Setup()
        {
            _syncService = new Mock<IViewConfigurationSyncService>();
            _logger = new Mock<ILogger>();

            _worker = new SyncWorker(
                _syncService.Object,
                IntervalSeconds,
                MaxBackoffSeconds,
                _logger.Object);
        }

        
      [TestMethod]
        public void RunOnce_WhenSyncSucceeds_ReturnsBaseInterval()
        {
            _syncService.Setup(x => x.SyncAll());

            var delay = _worker.RunOnce();

            Assert.AreEqual(IntervalSeconds, delay);
        }

        [TestMethod]
        public void RunOnce_WhenSyncSucceedsAfterFailures_ResetsBackoffAndLogsRecovery()
        {
            _syncService
                .SetupSequence(x => x.SyncAll())
                .Throws(new TimeoutException())
                .Pass();

            _worker.RunOnce();
            var delay = _worker.RunOnce();

            Assert.AreEqual(IntervalSeconds, delay);
        }

        [TestMethod]
        public void RunOnce_WhenCommunicationExceptionThrown_AppliesExponentialBackoff()
        {
            _syncService
                .Setup(x => x.SyncAll())
                .Throws(new CommunicationException());

            var delay = _worker.RunOnce();

            Assert.AreEqual(IntervalSeconds * 2, delay);
        }

        [TestMethod]
        public void RunOnce_WhenTimeoutExceptionThrown_AppliesExponentialBackoff()
        {
            _syncService
                .Setup(x => x.SyncAll())
                .Throws(new TimeoutException());

            var delay = _worker.RunOnce();

            Assert.AreEqual(IntervalSeconds * 2, delay);
        }

        [TestMethod]
        public void RunOnce_WhenUnexpectedExceptionThrown_AppliesExponentialBackoff()
        {
            _syncService
                .Setup(x => x.SyncAll())
                .Throws(new InvalidOperationException());

            var delay = _worker.RunOnce();

            Assert.AreEqual(IntervalSeconds * 2, delay);
        }

        [TestMethod]
        public void RunOnce_WhenMultipleConsecutiveFailures_BackoffDoesNotExceedMaxBackoffSeconds()
        {
            _syncService
                .Setup(x => x.SyncAll())
                .Throws(new TimeoutException());

            var delay = 0;
            for (var i = 0; i < 10; i++)
            {
                delay = _worker.RunOnce();
            }

            Assert.AreEqual(MaxBackoffSeconds, delay);
        }
    }
}