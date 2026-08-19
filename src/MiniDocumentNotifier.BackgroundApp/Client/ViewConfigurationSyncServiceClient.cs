using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using MiniDocumentNotifier.Application.Sync;
using MiniDocumentNotifier.Contracts.ServiceContracts;
using MiniDocumentNotifier.Contracts.ViewConfigurationContracts;

namespace MiniDocumentNotifier.BackgroundApp.Client
{
    public class ViewConfigurationSyncServiceClient : IViewConfigurationSyncServiceClient, IDisposable
    {
        private readonly Lazy<ChannelFactory<IViewConfigurationService>> _channelFactory;

        public ViewConfigurationSyncServiceClient()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(ConfigurationManager.AppSettings["ViewConfigurationSyncServiceUrl"]);
            _channelFactory = new Lazy<ChannelFactory<IViewConfigurationService>>(() =>
                new ChannelFactory<IViewConfigurationService>(binding, endpoint));
        }

        internal ViewConfigurationSyncServiceClient(Lazy<ChannelFactory<IViewConfigurationService>> channelFactory)
        {
            _channelFactory = channelFactory;
        }

        public List<ViewConfigurationDto> GetAllViewConfigurations()
        {
            var channel = _channelFactory.Value.CreateChannel();
            try
            {
                var result = channel.GetViewConfigurations();
                ((IClientChannel)channel).Close();
                return result;
            }
            catch (CommunicationException)
            {
                ((IClientChannel)channel).Abort();
                throw;
            }
            catch (TimeoutException)
            {
                ((IClientChannel)channel).Abort();
                throw;
            }
        }

        public void Dispose()
        {
            if (_channelFactory.IsValueCreated) _channelFactory.Value.Close();
        }
    }
}