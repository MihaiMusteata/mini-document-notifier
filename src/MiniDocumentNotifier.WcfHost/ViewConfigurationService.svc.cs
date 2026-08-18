using System;
using System.Collections.Generic;
using System.ServiceModel;
using MiniDocumentNotifier.Application.ViewConfiguration;
using MiniDocumentNotifier.Contracts.ServiceContracts;
using MiniDocumentNotifier.Contracts.ViewConfigurationContracts;
using MiniDocumentNotifier.Domain.Abstractions;

namespace MiniDocumentNotifier.WcfHost
{
    public class ViewConfigurationService : IViewConfigurationService
    {
        private readonly IViewConfigurationQueryService  _viewConfigurationQueryService;
        private readonly ILogger _logger;

        public ViewConfigurationService(IViewConfigurationQueryService viewConfigurationQueryService, ILogger logger)
        {
            _viewConfigurationQueryService = viewConfigurationQueryService;
            _logger = logger;
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
                _logger.Error("Failed to load view configurations.", ex);
                throw new FaultException<ViewConfigurationFault>(
                    new ViewConfigurationFault { Message = "View configurations could not be loaded." },
                    new FaultReason("View configuration query failed."));
            }
        }
    }
}