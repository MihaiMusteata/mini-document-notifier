using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;

namespace MiniDocumentNotifier.Persistence.Repositories
{
    public class InstitutionRepository : BaseRepository, IInstitutionRepository
    {
        public List<InstitutionEntity> GetAll()
        {
            var institutions = new List<InstitutionEntity>();

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand("dbo.Institution_GetAll", connection))
            {
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
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand("dbo.Institution_GetById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(SetParameter("@Id", institutionId, SqlDbType.Int));

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