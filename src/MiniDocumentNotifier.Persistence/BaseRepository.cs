using System;
using System.Data;
using MiniDocumentNotifier.Persistence.SqlConnFactory;

namespace MiniDocumentNotifier.Persistence
{
    public class BaseRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        protected BaseRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected IDbConnection CreateConnection() => _connectionFactory.CreateConnection();

        protected IDataParameter SetParameter(
            IDbCommand command,
            string parameterName,
            object parameterValue,
            DbType dbType)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            parameter.Value = parameterValue ?? DBNull.Value;
            parameter.Direction = ParameterDirection.Input;
            return parameter;
        }

        protected IDataParameter SetOutputParameter(
            IDbCommand command,
            string parameterName,
            DbType dbType)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            parameter.Direction = ParameterDirection.Output;
            return parameter;
        }
    }
}