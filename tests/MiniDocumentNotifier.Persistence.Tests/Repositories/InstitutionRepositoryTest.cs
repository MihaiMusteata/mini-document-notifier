using System.Data;
using System.Linq;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;
using MiniDocumentNotifier.Persistence.Repositories;
using MiniDocumentNotifier.Persistence.SqlConnFactory;
using Moq;
using Newtonsoft.Json.Linq;

namespace MiniDocumentNotifier.Persistence.Tests.Repositories
{
    [TestClass]
    public class InstitutionRepositoryTest
    {
        private Mock<IDbConnectionFactory> _connectionFactory;
        private Mock<IDbConnection> _connection;
        private Mock<IDbCommand> _command;
        private Mock<IDataParameterCollection> _parameters;
        private Mock<IDataReader> _reader;
        private Mock<ILogger> _logger;

        private Fixture _fixture;

        private IInstitutionRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();

            _connectionFactory = new Mock<IDbConnectionFactory>();
            _connection = new Mock<IDbConnection>();
            _command = new Mock<IDbCommand>();
            _parameters = new Mock<IDataParameterCollection>();
            _reader = new Mock<IDataReader>();
            _logger = new Mock<ILogger>();

            _connectionFactory.Setup(x => x.CreateConnection()).Returns(_connection.Object);
            _connection.Setup(x => x.CreateCommand()).Returns(_command.Object);
            _command.Setup(x => x.Parameters).Returns(_parameters.Object);
            _command.Setup(x => x.CreateParameter()).Returns(() => new FakeParameter());
            _command.Setup(x => x.ExecuteReader()).Returns(_reader.Object);

            _repository = new InstitutionRepository(_connectionFactory.Object, _logger.Object);
        }
        
        [TestMethod]
        public void GetAll_ReturnsInstitutions()
        {
            var expected = _fixture.CreateMany<InstitutionEntity>(2).ToList();

            var callCount = 0;
            _reader.Setup(x => x.Read()).Returns(() => callCount < expected.Count && callCount++ >= 0);
            _reader.Setup(x => x["Id"]).Returns(() => expected[callCount - 1].Id);
            _reader.Setup(x => x["Code"]).Returns(() => expected[callCount - 1].Code);
            _reader.Setup(x => x["Name"]).Returns(() => expected[callCount - 1].Name);
            
            var result = _repository.GetAll();
            Assert.HasCount(expected.Count, result);
            CollectionAssert.AreEqual(
                expected.Select(x => (x.Id, x.Code, x.Name)).ToList(),
                result.Select(x => (x.Id, x.Code, x.Name)).ToList());
        }

        [TestMethod]
        public void GetById_WhenFound_ReturnsInstitution()
        {
            var expected = _fixture.Create<InstitutionEntity>();
            
            _reader.Setup(x => x.Read()).Returns(true);
            _reader.Setup(x => x["Id"]).Returns(expected.Id);
            _reader.Setup(x => x["Code"]).Returns(expected.Code);
            _reader.Setup(x => x["Name"]).Returns(expected.Name);
            
            var result = _repository.GetById(expected.Id);
            
            Assert.IsNotNull(result);
            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(expected),
                    JToken.FromObject(result)));
        }
        
        [TestMethod]
        public void GetById_WhenNotFound_ReturnsInstitution()
        {
            var institutionId = _fixture.Create<int>();
            _reader.Setup(x => x.Read()).Returns(false);
            
            var result = _repository.GetById(institutionId);
            
            Assert.IsNull(result);
        }
        

    }
}