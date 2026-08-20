using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Contracts.DocumentUploadContracts;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Infrastructure.ServiceClient;
using MiniDocumentNotifier.WpfControls.ViewModels;
using Moq;

namespace MiniDocumentNotifier.WpfControls.Tests.ViewModels
{
    [TestClass]
    public class DocumentUploadViewModelTest
    {
        private Mock<IDocumentNotifierServiceClient> _client;
        private Mock<IFileStorage> _fileStorage;
        private Mock<IFileMetadata> _fileMetadata;
        private Fixture _fixture;

        private const int InstitutionId = 10;
        private const string TestFilePath = @"C:\test\test.pdf";


        private DocumentUploadViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _client = new Mock<IDocumentNotifierServiceClient>();
            _fileStorage = new Mock<IFileStorage>();
            _fileMetadata = new Mock<IFileMetadata>();

            _fixture = new Fixture();

            _viewModel = new DocumentUploadViewModel(_client.Object, _fileStorage.Object, InstitutionId)
            {
                FilePath = TestFilePath,
                SelectedType = DocumentType.Statement
            };
        }

        [TestMethod]
        [DataRow(@"C:\test\test.pdf", "test.pdf")]
        [DataRow(@"C:\test\doc123.pdf", "doc123.pdf")]
        [DataRow("", "")]
        public void FilePath_Set_UpdatesDocumentName(string path, string expectedName)
        {
            _viewModel.FilePath = path;

            Assert.AreEqual(expectedName, _viewModel.DocumentName);
        }

        [TestMethod]
        public async Task UploadAsync_ValidFile_Success()
        {
            _fileMetadata.Setup(m => m.Length).Returns(1024);
            _fileStorage.Setup(x => x.GetInfo(TestFilePath)).Returns(_fileMetadata.Object);

            var fileBytes = _fixture.CreateMany<byte>(3).ToArray();

            _fileStorage.Setup(x => x.ReadAllBytes(TestFilePath))
                .Returns(fileBytes);

            DocumentUploadRequest capturedRequest = null;

            _client.Setup(c => c.UploadDocument(It.IsAny<DocumentUploadRequest>()))
                .Callback<DocumentUploadRequest>(r => capturedRequest = r)
                .Returns(new DocumentUploadResult());


            await _viewModel.UploadAsync();

            Assert.IsNotNull(capturedRequest);
            Assert.AreEqual(InstitutionId, capturedRequest.InstitutionId);
            Assert.AreEqual("test.pdf", capturedRequest.FileName);
            Assert.AreEqual(DocumentType.Statement, capturedRequest.Type);
            CollectionAssert.AreEqual(fileBytes, capturedRequest.Content);
        }

        [TestMethod]
        public async Task UploadAsync_TooLargeFile_ShowsErrorAndDoesNotUpload()
        {
            _fileMetadata.Setup(m => m.Length).Returns(6 * 1024 * 1024);
            _fileStorage.Setup(x => x.GetInfo(TestFilePath)).Returns(_fileMetadata.Object);

            await _viewModel.UploadAsync();

            Assert.AreEqual("Upload failed: The file is too large. Maximum size is 5MB", _viewModel.StatusMessage);

            _client.Verify(c => c.UploadDocument(It.IsAny<DocumentUploadRequest>()), Times.Never);

            Assert.IsFalse(_viewModel.IsUploading);
        }

        [TestMethod]
        public async Task UploadAsync_TooLargeFile_ShowsFaultExceptionAndDoesNotUpload()
        {
            _fileMetadata.Setup(m => m.Length).Returns(1024);
            _fileStorage.Setup(x => x.GetInfo(TestFilePath)).Returns(_fileMetadata.Object);

            var uploadCompletedRaised = false;
            _viewModel.UploadCompleted += () => uploadCompletedRaised = true;

            var fault = new FaultException<DocumentUploadFault>(
                new DocumentUploadFault { Message = "Failed....." },
                new FaultReason("Failed"));

            _client.Setup(c => c.UploadDocument(It.IsAny<DocumentUploadRequest>())).Throws(fault);

            await _viewModel.UploadAsync();

            Assert.AreEqual("Upload failed: Failed.....", _viewModel.StatusMessage);
            Assert.IsFalse(uploadCompletedRaised);
        }
        
        [TestMethod]
        public async Task UploadAsync_CommunicationException_SetsCommunicationErrorStatus()
        {
            _fileMetadata.Setup(m => m.Length).Returns(1024);
            _fileStorage.Setup(x => x.GetInfo(TestFilePath)).Returns(_fileMetadata.Object);
            
            _client.Setup(c => c.UploadDocument(It.IsAny<DocumentUploadRequest>()))
                .Throws(new CommunicationException("channel faulted"));

            await _viewModel.UploadAsync();

            Assert.AreEqual("Communication error with the service.", _viewModel.StatusMessage);
        }
    }
}