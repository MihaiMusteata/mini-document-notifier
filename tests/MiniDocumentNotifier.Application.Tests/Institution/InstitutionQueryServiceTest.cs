using System.Linq;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Institution;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;
using Moq;

namespace MiniDocumentNotifier.Application.Tests.Institution
{
    [TestClass]
    public class InstitutionQueryServiceTest
    {
        private Mock<IInstitutionRepository> _institutionRepository;
        private IInstitutionQueryService _institutionQueryService;
        private Fixture _fixture;

        [TestInitialize]
        public void Setup()
        {
            _institutionRepository = new Mock<IInstitutionRepository>();
            _institutionQueryService = new InstitutionQueryService(_institutionRepository.Object);
            _fixture = new Fixture();
        }

        [TestMethod]
        public void GetAllInstitutions_ReturnsMappedInstitutions()
        {
            var institutions = _fixture.CreateMany<InstitutionEntity>(3).ToList();

            _institutionRepository
                .Setup(repo => repo.GetAll())
                .Returns(institutions);

            var result = _institutionQueryService.GetAll();

            Assert.AreEqual(institutions.Count, result.Count);

            CollectionAssert.AreEqual(
                institutions.Select(x => (x.Id, x.Name, x.Code)).ToList(),
                result.Select(x => (x.Id, x.Name, x.Code)).ToList());

            _institutionRepository.Verify(x => x.GetAll(), Times.Once);
        }
    }
}