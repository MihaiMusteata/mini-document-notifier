using System;
using System.IO;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Contracts.DocumentUploadContracts;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Domain.Repositories;
using Moq;

namespace MiniDocumentNotifier.Application.Tests.Document
{
    [TestClass]
    public class DocumentUploadServiceTest
    {
        private Mock<IDocumentRepository> _documentRepository;
        private Mock<IFileStorage> _fileStorage;
        private string _storageFilePath;
        private Fixture _fixture;
        private IDocumentUploadService _documentUploadService;

        [TestInitialize]
        public void Setup()
        {
            _documentRepository = new Mock<IDocumentRepository>();
            _fileStorage = new Mock<IFileStorage>();
            _fixture = new Fixture();
            _storageFilePath = _fixture.Create<string>();
            _documentUploadService = new DocumentUploadService(
                _documentRepository.Object,
                _fileStorage.Object,
                _storageFilePath);
        }

        [TestMethod]
        public void UploadDocument_WithValidFile_ReturnDocumentId()
        {
            var request = _fixture.Create<DocumentUploadRequest>();
            var documentIdResult = _fixture.Create<int>();

            _documentRepository
                .Setup(x => x.Insert(It.IsAny<DocumentEntity>()))
                .Returns(documentIdResult);

            var result = _documentUploadService.Upload(request);

            Assert.AreEqual(documentIdResult, result);

            var expectedPath = Path.Combine(_storageFilePath, request.InstitutionId.ToString());
            _fileStorage.Verify(x => x.CreateDirectory(expectedPath), Times.Once);

            _fileStorage.Verify(
                x => x.WriteAllBytes(
                    It.IsAny<string>(),
                    request.Content),
                Times.Once);

            _documentRepository.Verify(
                x => x.Insert(
                    It.Is<DocumentEntity>(document =>
                        document.InstitutionId == request.InstitutionId &&
                        document.Name == request.FileName &&
                        document.Type == request.Type &&
                        document.Status == DocumentStatus.New)),
                Times.Once);
        }

        [TestMethod]
        public void UploadDocument_WithNullContent_ThrowsArgumentException()
        {
            var request = _fixture
                .Build<DocumentUploadRequest>()
                .With(x => x.Content, (byte[])null)
                .Create();

            Assert.ThrowsExactly<ArgumentException>(() => _documentUploadService.Upload(request));
        }
    }
}