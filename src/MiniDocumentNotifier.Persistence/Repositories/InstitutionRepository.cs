using System.Collections.Generic;
using System.Data;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;
using MiniDocumentNotifier.Persistence.SqlConnFactory;

namespace MiniDocumentNotifier.Persistence.Repositories
{
    public class InstitutionRepository : BaseRepository, IInstitutionRepository
    {
        private readonly ILogger _logger;

        public InstitutionRepository(IDbConnectionFactory connectionFactory, ILogger logger) : base(connectionFactory)
        {
            _logger = logger;
        }

        public List<InstitutionEntity> GetAll()
        {
            _logger.Info("Getting all institutions");
            var institutions = new List<InstitutionEntity>();

            using (var connection = CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "dbo.Institution_GetAll";
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        institutions.Add(new InstitutionEntity
                        {
                            Id = (int)reader["Id"],
                            Code = (string)reader["Code"],
                            Name = (string)reader["Name"]
                        });
                    }
                }
            }

            return institutions;
        }

        public InstitutionEntity GetById(int institutionId)
        {
            using (var connection = CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "dbo.Institution_GetById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(SetParameter(command, "@Id", institutionId, DbType.Int32));

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new InstitutionEntity
                        {
                            Id = (int)reader["Id"],
                            Code = (string)reader["Code"],
                            Name = (string)reader["Name"]
                        };
                    }
                }
            }

            return null;
        }
    }
}