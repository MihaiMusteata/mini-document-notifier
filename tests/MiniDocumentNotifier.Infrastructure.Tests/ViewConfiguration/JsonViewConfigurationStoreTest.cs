using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Infrastructure.ViewConfiguration;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniDocumentNotifier.Infrastructure.Tests.ViewConfiguration
{
    [TestClass]
    public class JsonViewConfigurationStoreTest
    {
        private Mock<ILogger> _logger;
        private Mock<IFileStorage> _fileStorage;
        private Fixture _fixture;

        private readonly TimeSpan _stalenessThreshold = TimeSpan.FromHours(1);
        private const string Path = @"C:\test\test.pdf";
        private IViewConfigurationStore _viewConfigurationStore;

        [TestInitialize]
        public void Setup()
        {
            _fileStorage = new Mock<IFileStorage>();
            _fixture = new Fixture();
            _logger = new Mock<ILogger>();

            _viewConfigurationStore = new JsonViewConfigurationStore(
                _stalenessThreshold,
                Path,
                _logger.Object,
                _fileStorage.Object
            );
        }

        [TestMethod]
        public void Load_WhenFileDoesNotExist_ReturnsFileExistsFalse()
        {
            _fileStorage.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

            var result = _viewConfigurationStore.Load();

            var expected = new ViewConfigurationResult
            {
                FileExists = false,
                IsStale = false,
                Institutions = new List<InstitutionViewConfiguration>()
            };

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(expected),
                    JToken.FromObject(result)));
        }

        [TestMethod]
        public void Load_WhenFailedToParse_ReturnsEmptyInstitutions()
        {
            _fileStorage.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            _fileStorage.Setup(x => x.GetLastWriteTimeUtc(It.IsAny<string>())).Returns(DateTime.UtcNow);
            _fileStorage.Setup(x => x.ReadAllText(It.IsAny<string>())).Throws<Exception>();

            var result = _viewConfigurationStore.Load();

            var expected = new ViewConfigurationResult
            {
                FileExists = true,
                IsStale = false,
                Institutions = new List<InstitutionViewConfiguration>()
            };

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(expected),
                    JToken.FromObject(result)));
        }


        [TestMethod]
        public void Load_WithEverythingValid_ReturnsInstitutions()
        {
            _fileStorage.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            _fileStorage.Setup(x => x.GetLastWriteTimeUtc(It.IsAny<string>())).Returns(DateTime.UtcNow);

            var institutions = _fixture
                .CreateMany<InstitutionViewConfiguration>(3)
                .ToList();

            var json = JsonConvert.SerializeObject(institutions,
                new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                    {
                        NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
                    }
                });

            _fileStorage.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(json);

            var result = _viewConfigurationStore.Load();

            var expected = new ViewConfigurationResult
            {
                FileExists = true,
                IsStale = false,
                Institutions = institutions
            };

            Assert.HasCount(expected.Institutions.Count, result.Institutions);

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(expected),
                    JToken.FromObject(result)));
        }

        [TestMethod]
        public void Load_WhenFileIsStale_ReturnsIsStaleTrue()
        {
            _fileStorage
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(true);

            _fileStorage
                .Setup(x => x.GetLastWriteTimeUtc(It.IsAny<string>()))
                .Returns(DateTime.UtcNow - _stalenessThreshold - TimeSpan.FromMinutes(1));

            var institutions = _fixture
                .CreateMany<InstitutionViewConfiguration>(3)
                .ToList();

            var json = JsonConvert.SerializeObject(institutions,
                new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                    {
                        NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
                    }
                });

            _fileStorage.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(json);

            var result = _viewConfigurationStore.Load();

            var expected = new ViewConfigurationResult
            {
                FileExists = true,
                IsStale = true,
                Institutions = institutions
            };

            Assert.HasCount(expected.Institutions.Count, result.Institutions);

            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(expected),
                    JToken.FromObject(result)));

            _logger.Verify(x => x.Warning(It.Is<string>(msg => msg.Contains("is stale"))), Times.Once);
        }
    }
}