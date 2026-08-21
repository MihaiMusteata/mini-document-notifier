using System;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.RegistryWrapper;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Infrastructure.Preferences;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniDocumentNotifier.Infrastructure.Tests.Preferences
{
    [TestClass]
    public class RegistryUserPreferencesStoreTest
    {
        private Mock<IRegistryStore> _registryStore;
        private Mock<ILogger> _logger;
        private Fixture _fixture;

        private const string TestKeyPath = @"Tests\Key\Path\UserPreferences";
        private IUserPreferencesStore _userPreferencesStore;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _logger = new Mock<ILogger>();
            _registryStore = new Mock<IRegistryStore>();

            _userPreferencesStore = new RegistryUserPreferencesStore(
                TestKeyPath,
                _logger.Object,
                _registryStore.Object);
        }

        [TestMethod]
        public void Load_ValueDoesNotExist_ReturnsDefaultsAndSaves()
        {
            var defaults = UserPreferences.CreateDefault();

            _registryStore.Setup(x => x.GetValue(TestKeyPath, "UserPreferences")).Returns((string)null);

            var result = _userPreferencesStore.Load();

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(defaults),
                    JToken.FromObject(result)));

            _registryStore.Verify(x => x.SetValue(TestKeyPath, "UserPreferences", It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Load_ValidJson_ReturnsDeserializedPreferences()
        {
            var expected = _fixture.Create<UserPreferences>();
            var json = JsonConvert.SerializeObject(expected);

            _registryStore.Setup(x => x.GetValue(TestKeyPath, "UserPreferences"))
                .Returns(json);

            var result = _userPreferencesStore.Load();

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(expected),
                    JToken.FromObject(result)));
        }


        [TestMethod]
        public void Load_InvalidJson_ReturnsDefaults()
        {
            _registryStore.Setup(x => x.GetValue(TestKeyPath, "UserPreferences"))
                .Returns("not valid json");

            var result = _userPreferencesStore.Load();

            var defaults = UserPreferences.CreateDefault();

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(defaults),
                    JToken.FromObject(result)));
        }

        [TestMethod]
        public void Load_RegistryThrowsException_ReturnsDefaults()
        {
            var ex = new Exception();

            _registryStore.Setup(x => x.GetValue(TestKeyPath, "UserPreferences"))
                .Throws(ex);

            var result = _userPreferencesStore.Load();

            var defaults = UserPreferences.CreateDefault();

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(defaults),
                    JToken.FromObject(result)));
        }

        [TestMethod]
        public void Save_ValidPreferences_WritesToRegistry()
        {
            var preferences = _fixture.Create<UserPreferences>();

            _userPreferencesStore.Save(preferences);

            _registryStore.Verify(x => x.SetValue(TestKeyPath, "UserPreferences", It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Save_SetValueThrowsException_DoesNotThrow()
        {
            var preferences = _fixture.Create<UserPreferences>();
            var ex = new Exception();

            _registryStore.Setup(x => x.SetValue(TestKeyPath, "UserPreferences", It.IsAny<string>())).Throws(ex);

            _userPreferencesStore.Save(preferences);

            _logger.Verify(
                x => x.Error($"Failed to write user preferences to registry key '{TestKeyPath}' (source: Registry).",
                    ex), Times.Once);
        }
    }
}