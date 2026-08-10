using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using MiniDocumentNotifier.Contracts;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.Contracts.InstitutionContracts;

namespace MiniDocumentNotifier.WinForms.Services
{
    public class DocumentNotifierServiceClient : IDisposable
    {
        private readonly ChannelFactory<IDocumentNotifierService> _channelFactory;

        public DocumentNotifierServiceClient()
        {
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(ConfigurationManager.AppSettings["DocumentNotifierServiceUrl"]);
            _channelFactory = new ChannelFactory<IDocumentNotifierService>(binding, endpoint);
        }

        public LoginResult Login(LoginRequest request) => _channelFactory.CreateChannel().Login(request);
        public List<InstitutionDto> GetInstitutions() => _channelFactory.CreateChannel().GetInstitutions();

        public void Dispose() => _channelFactory.Close();
    }
}