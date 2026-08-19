using System.Linq;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Application.Sync;
using MiniDocumentNotifier.Contracts.ViewConfigurationContracts;
using MiniDocumentNotifier.Domain.Abstractions;
using Moq;

namespace MiniDocumentNotifier.Application.Tests.Sync
{
    [TestClass]
    public class ViewConfigurationSyncServiceTest
    {
        private Mock<IViewConfigurationSyncServiceClient> _client;
        private Mock<IFileStorage> _fileStorage;
        private Mock<ILogger> _logger;

        private IViewConfigurationSyncService _viewConfigurationSyncService;

        private Fixture _fixture;
        private string _outputFilePath;

        [TestInitialize]
        public void Setup()
        {
            _client = new Mock<IViewConfigurationSyncServiceClient>();
            _fileStorage = new Mock<IFileStorage>();
            _logger = new Mock<ILogger>();

            _fixture = new Fixture();
            _outputFilePath = _fixture.Create<string>();

            _viewConfigurationSyncService = new ViewConfigurationSyncService(
                _client.Object,
                _fileStorage.Object,
                _outputFilePath,
                _logger.Object);
        }

        [TestMethod]
        public void SyncAll_Test()
        {
            var viewConfigurations = _fixture
                .Build<ViewConfigurationDto>()
                .With(x => x.VisibleColumns, "[\"Name\",\"Type\"]")
                .With(x => x.ActiveCategories, "[\"Invoices\"]")
                .CreateMany(3)
                .ToList();

            _client
                .Setup(x => x.GetAllViewConfigurations())
                .Returns(viewConfigurations);

            _viewConfigurationSyncService.SyncAll();

            _fileStorage.Verify(x => x.WriteAllText(_outputFilePath, It.IsAny<string>()), Times.Once);
        }
    }
}