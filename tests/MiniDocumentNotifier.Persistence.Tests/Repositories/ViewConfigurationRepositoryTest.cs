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

namespace MiniDocumentNotifier.Persistence.Tests.Repositories
{
    [TestClass]
    public class ViewConfigurationRepositoryTest
    {
        private Mock<IDbConnectionFactory> _connectionFactory;
        private Mock<IDbConnection> _connection;
        private Mock<IDbCommand> _command;
        private Mock<IDataParameterCollection> _parameters;
        private Mock<IDataReader> _reader;

        private Fixture _fixture;

        private IViewConfigurationRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();

            _connectionFactory = new Mock<IDbConnectionFactory>();
            _connection = new Mock<IDbConnection>();
            _command = new Mock<IDbCommand>();
            _parameters = new Mock<IDataParameterCollection>();
            _reader = new Mock<IDataReader>();

            _connectionFactory.Setup(x => x.CreateConnection()).Returns(_connection.Object);
            _connection.Setup(x => x.CreateCommand()).Returns(_command.Object);
            _command.Setup(x => x.Parameters).Returns(_parameters.Object);
            _command.Setup(x => x.CreateParameter()).Returns(() => new FakeParameter());
            _command.Setup(x => x.ExecuteReader()).Returns(_reader.Object);

            _repository = new  ViewConfigurationRepository(_connectionFactory.Object);
        }

        [TestMethod]
        public void GetAllWithInstitutions_ReturnsAllViewConfigurations()
        {
            var expected =  _fixture.CreateMany<ViewConfigurationEntity>(3).ToList();
            
            var callCount = 0;
            _reader.Setup(x => x.Read()).Returns(() => callCount < expected.Count && callCount++ >= 0);
            _reader.Setup(x => x["Id"]).Returns(() => expected[callCount - 1].Id);
            _reader.Setup(x => x["InstitutionId"]).Returns(() => expected[callCount - 1].InstitutionId);
            _reader.Setup(x => x["VisibleColumns"]).Returns(() => expected[callCount - 1].VisibleColumns);
            _reader.Setup(x => x["ActiveCategories"]).Returns(() => expected[callCount - 1].ActiveCategories);
            _reader.Setup(x => x["LastUpdatedDate"]).Returns(() => expected[callCount - 1].LastUpdatedDate);
            _reader.Setup(x => x["Institution_Id"]).Returns(() => expected[callCount - 1].Institution.Id);
            _reader.Setup(x => x["Institution_Code"]).Returns(() => expected[callCount - 1].Institution.Code);
            _reader.Setup(x => x["Institution_Name"]).Returns(() => expected[callCount - 1].Institution.Name);
            
            var result = _repository.GetAllWithInstitutions();
            
            CollectionAssert.AreEqual(
                expected.Select(x => (x.Id, x.InstitutionId, x.VisibleColumns, x.ActiveCategories, x.LastUpdatedDate)).ToList(),
                result.Select(x => (x.Id, x.InstitutionId, x.VisibleColumns, x.ActiveCategories, x.LastUpdatedDate)).ToList());

            CollectionAssert.AreEqual(
                expected.Select(x => (x.Institution.Id, x.Institution.Code, x.Institution.Name)).ToList(),
                result.Select(x => (x.Institution.Id, x.Institution.Code, x.Institution.Name)).ToList());
        }
    }
}