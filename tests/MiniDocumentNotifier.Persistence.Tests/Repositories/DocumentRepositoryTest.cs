using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Domain.Repositories;
using MiniDocumentNotifier.Persistence.Repositories;
using MiniDocumentNotifier.Persistence.SqlConnFactory;
using Moq;

namespace MiniDocumentNotifier.Persistence.Tests.Repositories
{
    [TestClass]
    public class DocumentRepositoryTest
    {
        private Mock<IDbConnectionFactory> _connectionFactory;
        private Mock<IDbConnection> _connection;
        private Mock<IDbCommand> _command;
        private Mock<IDataParameterCollection> _parameters;
        private Mock<IDataReader> _reader;

        private Fixture _fixture;

        private IDocumentRepository _repository;

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

            _repository = new DocumentRepository(_connectionFactory.Object);
        }


        [TestMethod]
        public void GetByInstitution_ReturnsMappedDocuments()
        {
            var institution = _fixture.Create<InstitutionEntity>();
            var expected = _fixture.CreateMany<DocumentEntity>(3).ToList();
            
            var callCount = 0;
            _reader.Setup(x => x.Read()).Returns(() => callCount < expected.Count && callCount++ >= 0);
            _reader.Setup(x => x["Id"]).Returns(() => expected[callCount - 1].Id);
            _reader.Setup(x => x["InstitutionId"]).Returns(() => expected[callCount - 1].InstitutionId);
            _reader.Setup(x => x["Name"]).Returns(() => expected[callCount - 1].Name);
            _reader.Setup(x => x["Status"]).Returns(() => (int)expected[callCount - 1].Status);
            _reader.Setup(x => x["Type"]).Returns(() => (int)expected[callCount - 1].Type);
            _reader.Setup(x => x["UploadDate"]).Returns(() => expected[callCount - 1].UploadDate);

            var result = _repository.GetByInstitution(institution.Id);
            
            Assert.HasCount(expected.Count, result);

            CollectionAssert.AreEqual(
                expected.Select(x => (x.Id, x.InstitutionId, x.Name, x.Status, x.Type, x.UploadDate)).ToList(),
                result.Select(x => (x.Id, x.InstitutionId, x.Name, x.Status, x.Type, x.UploadDate)).ToList());
        }

        [TestMethod]
        public void GetPaged_WithoutAllowedTypes_ReturnsPagedResult()
        {
            const int expectedTotalCount = 3;
            var expectedItems = _fixture.CreateMany<DocumentEntity>(expectedTotalCount).ToList();
            
            var callCount = 0;
            _reader.Setup(x => x.Read()).Returns(() => callCount < expectedItems.Count && callCount++ >= 0);
            _reader.Setup(x => x["Id"]).Returns(() => expectedItems[callCount - 1].Id);
            _reader.Setup(x => x["InstitutionId"]).Returns(() => expectedItems[callCount - 1].InstitutionId);
            _reader.Setup(x => x["Name"]).Returns(() => expectedItems[callCount - 1].Name);
            _reader.Setup(x => x["Status"]).Returns(() => (int)expectedItems[callCount - 1].Status);
            _reader.Setup(x => x["Type"]).Returns(() => (int)expectedItems[callCount - 1].Type);
            _reader.Setup(x => x["UploadDate"]).Returns(() => expectedItems[callCount - 1].UploadDate);

            _parameters.Setup(x => x.Add(It.IsAny<object>()))
                .Callback<object>(p =>
                {
                    var param = (IDbDataParameter)p;
                    if (param.ParameterName == "@TotalCount")
                        param.Value = expectedTotalCount;
                });

            var query = _fixture.Build<DocumentQuery>()
                .With(q => q.AllowedTypes, (List<DocumentType>)null)
                .Create();

            var result = _repository.GetPaged(query);
            
            Assert.HasCount(expectedItems.Count, result.Items);
            Assert.AreEqual(expectedTotalCount, result.TotalItems);

            CollectionAssert.AreEqual(
                expectedItems.Select(x => (x.Id, x.InstitutionId, x.Name, x.Status, x.Type, x.UploadDate)).ToList(),
                result.Items.Select(x => (x.Id, x.InstitutionId, x.Name, x.Status, x.Type, x.UploadDate)).ToList());
        }
        
        [TestMethod]
        public void GetPaged_WithAllowedTypes_ReturnsPagedResult()
        {
            const int expectedTotalCount = 3;
            var expectedItems = _fixture.CreateMany<DocumentEntity>(expectedTotalCount).ToList();
            
            var callCount = 0;
            _reader.Setup(x => x.Read()).Returns(() => callCount < expectedItems.Count && callCount++ >= 0);
            _reader.Setup(x => x["Id"]).Returns(() => expectedItems[callCount - 1].Id);
            _reader.Setup(x => x["InstitutionId"]).Returns(() => expectedItems[callCount - 1].InstitutionId);
            _reader.Setup(x => x["Name"]).Returns(() => expectedItems[callCount - 1].Name);
            _reader.Setup(x => x["Status"]).Returns(() => (int)expectedItems[callCount - 1].Status);
            _reader.Setup(x => x["Type"]).Returns(() => (int)expectedItems[callCount - 1].Type);
            _reader.Setup(x => x["UploadDate"]).Returns(() => expectedItems[callCount - 1].UploadDate);

            _parameters.Setup(x => x.Add(It.IsAny<object>()))
                .Callback<object>(p =>
                {
                    var param = (IDbDataParameter)p;
                    if (param.ParameterName == "@TotalCount")
                        param.Value = expectedTotalCount;
                });

            var allowedTypes = _fixture.CreateMany<DocumentType>(3).ToList();
            var query = _fixture.Build<DocumentQuery>()
                .With(q => q.AllowedTypes, allowedTypes)
                .Create();

            var result = _repository.GetPaged(query);
            
            Assert.HasCount(expectedItems.Count, result.Items);
            Assert.AreEqual(expectedTotalCount, result.TotalItems);

            CollectionAssert.AreEqual(
                expectedItems.Select(x => (x.Id, x.InstitutionId, x.Name, x.Status, x.Type, x.UploadDate)).ToList(),
                result.Items.Select(x => (x.Id, x.InstitutionId, x.Name, x.Status, x.Type, x.UploadDate)).ToList());
        }

        [TestMethod]
        public void GetPaged_WhenExceptionThrown_ReturnsEmptyResult()
        {
            _connection.Setup(x => x.Open()).Throws<Exception>();

            var query = _fixture.Build<DocumentQuery>()
                .With(q => q.AllowedTypes, (List<DocumentType>)null)
                .Create();

            var result = _repository.GetPaged(query);

            Assert.AreEqual(0, result.TotalItems);
            Assert.IsNull(result.Items);
        }

        [TestMethod]
        public void Insert_ReturnsGeneratedDocumentId()
        {
            var document = _fixture.Create<DocumentEntity>();
            var expectedId = _fixture.Create<int>();
      
            _parameters.Setup(x => x.Add(It.IsAny<object>()))
                .Callback<object>(p =>
                {
                    var param = (IDbDataParameter)p;
                    if (param.ParameterName == "@DocumentId")
                        param.Value = expectedId;
                });

            var result = _repository.Insert(document);

            Assert.AreEqual(expectedId, result);
        }
        
    }
}