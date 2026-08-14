using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MiniDocumentNotifier.Persistence
{
    public class BaseRepository
    {
        protected readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["MiniDocumentNotifierDb"].ConnectionString;

        protected SqlParameter SetParameter(string parameterName, object parameterValue, SqlDbType dbType)
        {
            var sqlParameter = new SqlParameter(parameterName, dbType)
            {
                Value = parameterValue,
                Direction = ParameterDirection.Input
            };

            return sqlParameter;
        }

        protected SqlParameter SetOutputParameter(string parameterName, SqlDbType dbType)
        {
            return new SqlParameter(parameterName, dbType)
            {
                Direction = ParameterDirection.Output
            };
        }
        
    }
}