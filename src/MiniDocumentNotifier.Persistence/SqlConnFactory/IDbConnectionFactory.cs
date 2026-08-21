using System.Data;

namespace MiniDocumentNotifier.Persistence.SqlConnFactory
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}