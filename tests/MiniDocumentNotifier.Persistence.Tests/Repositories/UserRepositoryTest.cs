using System.Data;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;
using MiniDocumentNotifier.Persistence.Repositories;
using MiniDocumentNotifier.Persistence.SqlConnFactory;
using Moq;
using Newtonsoft.Json.Linq;

namespace MiniDocumentNotifier.Persistence.Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTest
    {
        private Mock<IDbConnectionFactory> _connectionFactory;
        private Mock<IDbConnection> _connection;
        private Mock<IDbCommand> _command;
        private Mock<IDataParameterCollection> _parameters;
        private Mock<IDataReader> _reader;

        private Fixture _fixture;

        private IUserRepository _repository;

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

            _repository = new  UserRepository(_connectionFactory.Object);
        }

        [TestMethod]
        public void GetByUsernameAndInstitutionId_Found_ReturnsUser()
        {
            var expected = _fixture.Create<UserEntity>();
            
            _reader.Setup(x => x.Read()).Returns(true);
            _reader.Setup(x => x["Id"]).Returns(() => expected.Id);
            _reader.Setup(x => x["Username"]).Returns(() => expected.Username);
            _reader.Setup(x => x["PasswordHash"]).Returns(() => expected.PasswordHash);
            _reader.Setup(x => x["PasswordSalt"]).Returns(() => expected.PasswordSalt);
            _reader.Setup(x => x["InstitutionId"]).Returns(() => expected.InstitutionId);
            _reader.Setup(x => x["IsEnabled"]).Returns(() => expected.IsEnabled);
            
            var result = _repository.GetByUsernameAndInstitutionId(expected.Username, expected.InstitutionId);
            
            Assert.IsNotNull(result);
            Assert.IsTrue(
                JToken.DeepEquals(
                    JToken.FromObject(expected),
                    JToken.FromObject(result)));
        }

        [TestMethod]
        public void GetByUsernameAndInstitutionId_NotFound_ReturnsNull()
        {
            var expected = _fixture.Create<UserEntity>();
            
            _reader.Setup(x => x.Read()).Returns(false);
            
            var result = _repository.GetByUsernameAndInstitutionId(expected.Username, expected.InstitutionId);
            
            Assert.IsNull(result);
        }
        
        [TestMethod]
        public void Register_ExecutesNonQueryWithCorrectParameters()
        {
            var user = _fixture.Create<UserEntity>();

            IDbDataParameter usernameParam = null;
            IDbDataParameter passwordHashParam = null;
            IDbDataParameter passwordSaltParam = null;
            IDbDataParameter institutionIdParam = null;

            _parameters.Setup(x => x.Add(It.IsAny<object>()))
                .Callback<object>(p =>
                {
                    var param = (IDbDataParameter)p;
                    switch (param.ParameterName)
                    {
                        case "@Username": usernameParam = param; break;
                        case "@PasswordHash": passwordHashParam = param; break;
                        case "@PasswordSalt": passwordSaltParam = param; break;
                        case "@InstitutionId": institutionIdParam = param; break;
                    }
                });

            _repository.Register(user);

            Assert.AreEqual(user.Username, usernameParam.Value);
            Assert.AreEqual(user.PasswordHash, passwordHashParam.Value);
            Assert.AreEqual(user.PasswordSalt, passwordSaltParam.Value);
            Assert.AreEqual(user.InstitutionId, institutionIdParam.Value);
        }
    }
}