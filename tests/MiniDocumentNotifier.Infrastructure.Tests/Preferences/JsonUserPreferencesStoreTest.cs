using System;
using System.IO;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Infrastructure.Preferences;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniDocumentNotifier.Infrastructure.Tests.Preferences
{
    [TestClass]
    public class JsonUserPreferencesStoreTest
    {
        private Mock<IFileStorage> _fileStorage;
        private Mock<ILogger> _logger;
        private Fixture _fixture;

        private const string TestFilePath = @"C:\test\preferences.json";
        private const string TestDirPath = @"C:\test";

        private IUserPreferencesStore _userPreferencesStore;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _fileStorage = new Mock<IFileStorage>();
            _logger = new Mock<ILogger>();

            _userPreferencesStore = new JsonUserPreferencesStore(
                TestFilePath,
                _logger.Object,
                _fileStorage.Object);
        }


        [TestMethod]
        public void Load_FileDoesNotExist_ReturnsDefaultsAndSavesThem()
        {
            var defaults = UserPreferences.CreateDefault();

            _fileStorage.Setup(x => x.Exists(TestFilePath)).Returns(false);
            _fileStorage.Setup(x => x.GetDirectoryName(TestFilePath)).Returns(TestDirPath);
            _fileStorage.Setup(x => x.Exists(TestDirPath)).Returns(false);

            var result = _userPreferencesStore.Load();

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(defaults),
                    JToken.FromObject(result)));

            _fileStorage.Verify(x => x.CreateDirectory(TestDirPath), Times.Once);
            _fileStorage.Verify(x => x.WriteAllText(TestFilePath, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Load_ValidJson_ReturnsDeserializedPreferences()
        {
            var expected = _fixture.Create<UserPreferences>();
            var json = JsonConvert.SerializeObject(expected);

            _fileStorage.Setup(x => x.Exists(TestFilePath)).Returns(true);
            _fileStorage.Setup(x => x.ReadAllText(TestFilePath)).Returns(json);

            var result = _userPreferencesStore.Load();

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(expected),
                    JToken.FromObject(result)));
        }

        [TestMethod]
        public void Load_InvalidJson_ReturnsDefaults()
        {
            _fileStorage.Setup(x => x.Exists(TestFilePath)).Returns(true);
            _fileStorage.Setup(x => x.ReadAllText(TestFilePath)).Returns("not valid json");

            var result = _userPreferencesStore.Load();

            var defaults = UserPreferences.CreateDefault();

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(defaults),
                    JToken.FromObject(result)));
        }

        [TestMethod]
        public void Save_DirectoryAlreadyExists_DoesNotCreateDirectory()
        {
            var preferences = _fixture.Create<UserPreferences>();

            _fileStorage.Setup(x => x.GetDirectoryName(TestFilePath)).Returns(TestDirPath);
            _fileStorage.Setup(x => x.DirectoryExists(TestDirPath)).Returns(true);

            _userPreferencesStore.Save(preferences);

            _fileStorage.Verify(x => x.CreateDirectory(It.IsAny<string>()), Times.Never);
            _fileStorage.Verify(x => x.WriteAllText(TestFilePath, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Save_WriteThrowsException_DoesNotThrow()
        {
            var preferences = _fixture.Create<UserPreferences>();

            _fileStorage.Setup(x => x.GetDirectoryName(TestFilePath)).Returns(TestDirPath);
            _fileStorage.Setup(x => x.DirectoryExists(TestDirPath)).Returns(true);
            var ex = new Exception();
            _fileStorage.Setup(x => x.WriteAllText(TestFilePath, It.IsAny<string>())).Throws(ex);

            _userPreferencesStore.Save(preferences);

            _logger.Verify(
                x => x.Error($"Failed to write user preferences to '{TestFilePath}' (source: JSON).", ex),
                Times.Once);
        }
    }
}