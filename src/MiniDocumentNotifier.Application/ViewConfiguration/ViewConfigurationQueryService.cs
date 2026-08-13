using System.Collections.Generic;
using System.Linq;
using MiniDocumentNotifier.Contracts.ViewConfigurationContracts;
using MiniDocumentNotifier.Domain.Repositories;

namespace MiniDocumentNotifier.Application.ViewConfiguration
{
    public class ViewConfigurationQueryService : IViewConfigurationQueryService
    {
        private readonly IViewConfigurationRepository _viewConfigurationRepository;

        public ViewConfigurationQueryService(IViewConfigurationRepository viewConfigurationRepository)
        {
            _viewConfigurationRepository = viewConfigurationRepository;
        }

        public List<ViewConfigurationDto> GetAll()
        {
            return _viewConfigurationRepository.GetAllWithInstitutions()
                .Select(entity => new ViewConfigurationDto
                {
                    InstitutionId = entity.Institution.Id,
                    InstitutionCode = entity.Institution.Code,
                    VisibleColumns = entity.VisibleColumns,
                    ActiveCategories = entity.ActiveCategories
                })
                .ToList();
        }
    }
}