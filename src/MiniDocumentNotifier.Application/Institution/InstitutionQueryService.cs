using System.Collections.Generic;
using System.Linq;
using MiniDocumentNotifier.Contracts.InstitutionContracts;
using MiniDocumentNotifier.Domain.Repositories;

namespace MiniDocumentNotifier.Application.Institution
{
    public class InstitutionQueryService : IInstitutionQueryService
    {
        private readonly IInstitutionRepository _institutionRepository;

        public InstitutionQueryService(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        public List<InstitutionDto> GetAll()
        {
            return _institutionRepository.GetAll()
                .Select(entity => new InstitutionDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Code = entity.Code
                }).ToList();
        }
    }
}