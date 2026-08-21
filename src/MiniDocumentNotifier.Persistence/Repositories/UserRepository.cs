using System.Data;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;
using MiniDocumentNotifier.Persistence.SqlConnFactory;

namespace MiniDocumentNotifier.Persistence.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public UserEntity GetByUsernameAndInstitutionId(
            string username,
            int institutionId)
        {
            using (var connection = CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "dbo.User_GetByUsernameAndInstitution";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    SetParameter(command, "@Username", username, DbType.AnsiString));

                command.Parameters.Add(
                    SetParameter(command, "@InstitutionId", institutionId, DbType.Int32));

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserEntity
                        {
                            Id = (int)reader["Id"],
                            Username = (string)reader["Username"],
                            PasswordHash = (string)reader["PasswordHash"],
                            PasswordSalt = (string)reader["PasswordSalt"],
                            InstitutionId = (int)reader["InstitutionId"],
                            IsEnabled = (bool)reader["IsEnabled"]
                        };
                    }
                }
            }

            return null;
        }

        public void Register(UserEntity user)
        {
            using (var connection = CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "dbo.User_Register";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    SetParameter(command, "@Username", user.Username, DbType.AnsiString));

                command.Parameters.Add(
                    SetParameter(command, "@PasswordHash", user.PasswordHash, DbType.AnsiString));

                command.Parameters.Add(
                    SetParameter(command, "@PasswordSalt", user.PasswordSalt, DbType.AnsiString));

                command.Parameters.Add(
                    SetParameter(command, "@InstitutionId", user.InstitutionId, DbType.Int32));

                connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}