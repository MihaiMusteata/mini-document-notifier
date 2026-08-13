using System.Collections.Generic;
using MiniDocumentNotifier.Contracts.ViewConfigurationContracts;

namespace MiniDocumentNotifier.Application.Sync
{
    public interface IViewConfigurationSyncServiceClient
    {
        List<ViewConfigurationDto> GetAllViewConfigurations();
    }
}