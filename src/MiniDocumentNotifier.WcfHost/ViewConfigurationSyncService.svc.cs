using System;
using System.Collections.Generic;
using System.ServiceModel;
using MiniDocumentNotifier.Application.ViewConfiguration;
using MiniDocumentNotifier.Contracts.ServiceContracts;
using MiniDocumentNotifier.Contracts.ViewConfigurationContracts;

namespace MiniDocumentNotifier.WcfHost
{
    public class ViewConfigurationService : IViewConfigurationService
    {
        private readonly IViewConfigurationQueryService  _viewConfigurationQueryService;

        public ViewConfigurationService(IViewConfigurationQueryService viewConfigurationQueryService)
        {
            _viewConfigurationQueryService = viewConfigurationQueryService;
        }

        public List<ViewConfigurationDto> GetViewConfigurations()
        {
            try
            {
                var viewConfigs = _viewConfigurationQueryService.GetAll();
                return viewConfigs;
            }
            catch (Exception ex)
            {
                throw new FaultException<ViewConfigurationFault>(
                    new ViewConfigurationFault { Message = "Error getting view configurations" },
                    new FaultReason("Failed"));
            }
        }
    }
}