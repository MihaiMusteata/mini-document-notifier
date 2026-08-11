using MiniDocumentNotifier.Domain.Models;

namespace MiniDocumentNotifier.Domain.Abstractions
{
    public interface IViewConfigurationStore
    {
        ViewConfigurationResult Load();
    }
}