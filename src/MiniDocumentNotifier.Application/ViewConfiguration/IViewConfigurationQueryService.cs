using System.Collections.Generic;
using MiniDocumentNotifier.Contracts.ViewConfigurationContracts;

namespace MiniDocumentNotifier.Application.ViewConfiguration
{
    public interface IViewConfigurationQueryService
    {
        List<ViewConfigurationDto> GetAll();
    }
}