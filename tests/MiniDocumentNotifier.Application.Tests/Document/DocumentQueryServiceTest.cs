using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Domain.Repositories;
using Moq;

namespace MiniDocumentNotifier.Application.Tests.Document
{
    [TestClass]
    public class DocumentQueryServiceTest
    {
        private Mock<IDocumentRepository> _repository;
        private IDocumentQueryService _documentQueryService;
        private Fixture _fixture;

        [TestInitialize]
        public void Setup()
        {
            _repository = new Mock<IDocumentRepository>();
            _documentQueryService = new DocumentQueryService(_repository.Object);
            _fixture = new Fixture();
        }

        [TestMethod]
        public void GetByInstitution_ReturnsMappedDocuments()
        {
            var institutionId = _fixture.Create<int>();

            var documents = _fixture.CreateMany<DocumentEntity>(3).ToList();

            _repository
                .Setup(x => x.GetByInstitution(institutionId))
                .Returns(documents);

            var result = _documentQueryService.GetByInstitution(institutionId);

            var expected = documents
                .Select(x => new { x.Id, x.Name, x.Type, x.Status, x.UploadDate })
                .ToList();

            var actual = result
                .Select(x => new { x.Id, x.Name, x.Type, x.Status, x.UploadDate })
                .ToList();

            CollectionAssert.AreEqual(expected, actual);

            _repository.Verify(
                x => x.GetByInstitution(institutionId),
                Times.Once);
        }


        [TestMethod]
        public void GetPaged_ReturnsPagedDocuments()
        {
            var query = _fixture.Create<DocumentQuery>();

            var documents = _fixture
                .Build<PagedResult<DocumentEntity>>()
                .With(x => x.Items, _fixture.CreateMany<DocumentEntity>(3).ToList())
                .With(x => x.TotalItems, 3)
                .Create();

            _repository
                .Setup(x => x.GetPaged(It.IsAny<DocumentQuery>()))
                .Returns(documents);

            var result = _documentQueryService.GetPaged(query);

            Assert.AreEqual(documents.TotalItems, result.TotalItems);
            Assert.AreEqual(documents.Items.Count, result.Items.Count);

            var expected = documents.Items
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Type,
                    x.Status,
                    x.UploadDate
                })
                .ToList();

            var actual = result.Items
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Type,
                    x.Status,
                    x.UploadDate
                })
                .ToList();

            CollectionAssert.AreEqual(expected, actual);

            _repository.Verify(
                x => x.GetPaged(query),
                Times.Once);
        }
    }
}