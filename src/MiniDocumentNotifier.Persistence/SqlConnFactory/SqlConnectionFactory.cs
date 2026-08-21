using System.Data;

namespace MiniDocumentNotifier.Persistence.SqlConnFactory
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() => new System.Data.SqlClient.SqlConnection(_connectionString);
    }
}

