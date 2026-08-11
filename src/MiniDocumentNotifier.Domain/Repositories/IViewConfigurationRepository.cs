using System.Collections.Generic;
using MiniDocumentNotifier.Domain.Entities;

namespace MiniDocumentNotifier.Domain.Repositories
{
    public interface IViewConfigurationRepository
    {
        List<ViewConfigurationEntity> GetAllWithInstitutions();
    }
}