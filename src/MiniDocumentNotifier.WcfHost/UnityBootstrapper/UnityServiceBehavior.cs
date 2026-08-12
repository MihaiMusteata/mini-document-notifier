using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Unity;

namespace MiniDocumentNotifier.WcfHost.UnityBootstrapper
{
    public class UnityServiceBehavior : IServiceBehavior
    {
        private readonly IUnityContainer _container;

        public UnityServiceBehavior(IUnityContainer container)
        {
            _container = container;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                var cd = (ChannelDispatcher)channelDispatcher;
                foreach (var endpoint in cd.Endpoints)
                {
                    endpoint.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(_container, serviceDescription.ServiceType);
                }
            }
        }
    }
}