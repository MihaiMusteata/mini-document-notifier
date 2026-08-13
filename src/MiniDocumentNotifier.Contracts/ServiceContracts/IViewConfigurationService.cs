using System.Collections.Generic;
using System.ServiceModel;
using MiniDocumentNotifier.Contracts.ViewConfigurationContracts;

namespace MiniDocumentNotifier.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface IViewConfigurationService
    {
        [OperationContract]
        [FaultContract(typeof(ViewConfigurationFault))]
        List<ViewConfigurationDto> GetViewConfigurations();
    }
}