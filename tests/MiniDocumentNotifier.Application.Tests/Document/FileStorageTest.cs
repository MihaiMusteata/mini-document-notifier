using System;
using System.IO;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Document;

namespace MiniDocumentNotifier.Application.Tests.Document
{
    [TestClass]
    public class FileStorageTest
    {
        private string _tempRoot;
        private FileStorage _fileStorage;
        private Fixture _fixture;

        [TestInitialize]
        public void Setup()
        {
            _fileStorage = new FileStorage();
            _tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _fixture = new Fixture();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(_tempRoot))
                Directory.Delete(_tempRoot, recursive: true);
        }

        [TestMethod]
        public void CreateDirectory_WhenPathDoesNotExist_CreatesDirectory()
        {
            Assert.IsFalse(Directory.Exists(_tempRoot));

            _fileStorage.CreateDirectory(_tempRoot);

            Assert.IsTrue(Directory.Exists(_tempRoot));
        }

        [TestMethod]
        public void CreateDirectory_WhenPathAlreadyExists_DoesNotThrow()
        {
            Directory.CreateDirectory(_tempRoot);

            _fileStorage.CreateDirectory(_tempRoot);

            Assert.IsTrue(Directory.Exists(_tempRoot));
        }

        [TestMethod]
        public void WriteAllBytes_WritesContentToFile()
        {
            Directory.CreateDirectory(_tempRoot);
            var filePath = Path.Combine(_tempRoot, "test-file.txt");
            var content = _fixture.Create<byte[]>();

            _fileStorage.WriteAllBytes(filePath, content);

            Assert.IsTrue(File.Exists(filePath));
            var writtenContent = File.ReadAllBytes(filePath);
            CollectionAssert.AreEqual(content, writtenContent);
        }

        [TestMethod]
        public void WriteAlLText_WritesContentToFile()
        {
            Directory.CreateDirectory(_tempRoot);
            var filePath = Path.Combine(_tempRoot, "test-file.txt");
            var content = _fixture.Create<string>();

            _fileStorage.WriteAllText(filePath, content);

            Assert.IsTrue(File.Exists(filePath));
            var writtenContent = File.ReadAllText(filePath);
            Assert.AreEqual(content, writtenContent);
        }
    }
}