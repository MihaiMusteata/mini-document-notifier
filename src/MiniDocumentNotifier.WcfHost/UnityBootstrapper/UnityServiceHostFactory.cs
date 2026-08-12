using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace MiniDocumentNotifier.WcfHost.UnityBootstrapper
{
    public class UnityServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = base.CreateServiceHost(serviceType, baseAddresses);
            host.Description.Behaviors.Add(new UnityServiceBehavior(Bootstrapper.Container));
            
            return host;
        }
    }
}