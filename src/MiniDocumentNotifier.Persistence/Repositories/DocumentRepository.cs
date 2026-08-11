using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Domain.Repositories;

namespace MiniDocumentNotifier.Persistence.Repositories
{
    public class DocumentRepository : BaseRepository, IDocumentRepository
    {
        public List<DocumentEntity> GetByInstitution(int institutionId)
        {
            var documents = new List<DocumentEntity>();

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand("Document_GetByInstitution", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    SetParameter("@InstitutionId", institutionId, SqlDbType.Int)
                );

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        documents.Add(new DocumentEntity
                        {
                            Id = (int)reader["Id"],
                            InstitutionId = (int)reader["InstitutionId"],
                            Name = (string)reader["Name"],
                            Status = (DocumentStatus)reader["Status"],
                            Type = (DocumentType)reader["Type"],
                            UploadDate = (DateTime)reader["UploadDate"],
                        });
                    }
                }
            }

            return documents;
        }
    }
}