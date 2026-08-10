using System.Data;
using System.Data.SqlClient;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;

namespace MiniDocumentNotifier.Persistence.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserEntity GetByUsernameAndInstitutionId(
            string username,
            int institutionId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(
                       "dbo.User_GetByUsernameAndInstitution",
                       connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    SetParameter("@Username", username, SqlDbType.VarChar));

                command.Parameters.Add(
                    SetParameter("@InstitutionId", institutionId, SqlDbType.Int));

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
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(
                       "dbo.User_Register",
                       connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    SetParameter("@Username", user.Username, SqlDbType.VarChar));

                command.Parameters.Add(
                    SetParameter("@PasswordHash", user.PasswordHash, SqlDbType.VarChar));

                command.Parameters.Add(
                    SetParameter("@PasswordSalt", user.PasswordSalt, SqlDbType.VarChar));

                command.Parameters.Add(
                    SetParameter("@InstitutionId", user.InstitutionId, SqlDbType.Int));

                connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}