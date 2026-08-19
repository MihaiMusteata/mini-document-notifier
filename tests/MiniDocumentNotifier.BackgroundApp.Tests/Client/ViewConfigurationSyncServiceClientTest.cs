using System;
using System.Linq;
using System.ServiceModel;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.BackgroundApp.Client;
using MiniDocumentNotifier.Contracts.ServiceContracts;
using MiniDocumentNotifier.Contracts.ViewConfigurationContracts;
using Moq;

namespace MiniDocumentNotifier.BackgroundApp.Tests.Client
{
    [TestClass]
    public class ViewConfigurationSyncServiceClientTest
    {
        private Mock<IViewConfigurationService> _channel;
        private Mock<IClientChannel> _clientChannel;
        private Mock<ChannelFactory<IViewConfigurationService>> _channelFactory;
        private ViewConfigurationSyncServiceClient _client;
        private Fixture _fixture;

        [TestInitialize]
        public void Setup()
        {
            _channel = new Mock<IViewConfigurationService>();
            _clientChannel = _channel.As<IClientChannel>();
            _fixture = new Fixture();

            _channelFactory = new Mock<ChannelFactory<IViewConfigurationService>>(
                new BasicHttpBinding(),
                new EndpointAddress("http://localhost/fake"))
            {
                CallBase = true
            };

            _channelFactory
                .Setup(x => x.CreateChannel(It.IsAny<EndpointAddress>(), It.IsAny<Uri>()))
                .Returns(_channel.Object);

            var lazyFactory = new Lazy<ChannelFactory<IViewConfigurationService>>(() => _channelFactory.Object);

            _client = new ViewConfigurationSyncServiceClient(lazyFactory);
        }

        [TestMethod]
        public void GetAllViewConfigurations_WhenCallSucceeds_ReturnsResultAndClosesChannel()
        {
            var expectedResult = _fixture.CreateMany<ViewConfigurationDto>(3).ToList();

            _channel
                .Setup(x => x.GetViewConfigurations())
                .Returns(expectedResult);

            var result = _client.GetAllViewConfigurations();

            Assert.AreEqual(expectedResult, result);

            _clientChannel.Verify(x => x.Close(), Times.Once);
            _clientChannel.Verify(x => x.Abort(), Times.Never);
        }

        [TestMethod]
        public void GetAllViewConfigurations_WhenCommunicationExceptionThrown_AbortsChannelAndRethrows()
        {
            _channel
                .Setup(x => x.GetViewConfigurations())
                .Throws<CommunicationException>();

            Assert.ThrowsExactly<CommunicationException>(() => _client.GetAllViewConfigurations());

            _clientChannel.Verify(x => x.Abort(), Times.Once);
            _clientChannel.Verify(x => x.Close(), Times.Never);
        }

        [TestMethod]
        public void GetAllViewConfigurations_WhenTimeoutExceptionThrown_AbortsChannelAndRethrows()
        {
            _channel
                .Setup(x => x.GetViewConfigurations())
                .Throws<TimeoutException>();

            Assert.ThrowsExactly<TimeoutException>(() => _client.GetAllViewConfigurations());

            _clientChannel.Verify(x => x.Abort(), Times.Once);
            _clientChannel.Verify(x => x.Close(), Times.Never);
        }
    }
}