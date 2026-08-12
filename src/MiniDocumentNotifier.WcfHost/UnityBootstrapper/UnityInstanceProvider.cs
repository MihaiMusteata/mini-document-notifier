using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Unity;

namespace MiniDocumentNotifier.WcfHost.UnityBootstrapper
{
    public class UnityInstanceProvider : IInstanceProvider
    {
        private readonly IUnityContainer  _container;
        private readonly Type _serviceType;

        public UnityInstanceProvider(IUnityContainer  container, Type serviceType)
        {
            _container = container;
            _serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return _container.Resolve(_serviceType);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return GetInstance(instanceContext);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}