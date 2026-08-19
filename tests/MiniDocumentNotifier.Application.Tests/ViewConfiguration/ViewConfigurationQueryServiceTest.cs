using System.Linq;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Sync;
using MiniDocumentNotifier.Application.ViewConfiguration;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;
using Moq;

namespace MiniDocumentNotifier.Application.Tests.ViewConfiguration
{
    [TestClass]
    public class ViewConfigurationQueryServiceTest
    {
        private Mock<IViewConfigurationRepository> _viewConfigurationRepository;
        private Fixture _fixture;

        private IViewConfigurationQueryService _viewConfigurationQueryService;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _viewConfigurationRepository = new Mock<IViewConfigurationRepository>();
            _viewConfigurationQueryService = new ViewConfigurationQueryService(_viewConfigurationRepository.Object);
        }

        [TestMethod]
        public void GetAllViewConfigurations_ReturnsMappedViewConfigurations()
        {
            var viewConfigurations = _fixture.CreateMany<ViewConfigurationEntity>(3).ToList();

            _viewConfigurationRepository
                .Setup(repo => repo.GetAllWithInstitutions())
                .Returns(viewConfigurations);

            var result = _viewConfigurationQueryService.GetAll();

            Assert.AreEqual(viewConfigurations.Count, result.Count);

            CollectionAssert.AreEqual(
                viewConfigurations.Select(x => (x.Institution.Id, x.Institution.Code, x.VisibleColumns,
                    x.ActiveCategories, x.LastUpdatedDate)).ToList(),
                result.Select(x => (x.InstitutionId, x.InstitutionCode, x.VisibleColumns, x.ActiveCategories,
                    x.LastUpdatedDate)).ToList());
            
            _viewConfigurationRepository.Verify(repo => repo.GetAllWithInstitutions(), Times.Once);
        }
    }
}