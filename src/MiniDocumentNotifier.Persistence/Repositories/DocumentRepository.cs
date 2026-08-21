using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Domain.Repositories;
using MiniDocumentNotifier.Persistence.SqlConnFactory;

namespace MiniDocumentNotifier.Persistence.Repositories
{
    public class DocumentRepository : BaseRepository, IDocumentRepository
    {
        public DocumentRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public List<DocumentEntity> GetByInstitution(int institutionId)
        {
            var documents = new List<DocumentEntity>();

            using (var connection = CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "Document_GetByInstitution";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(SetParameter(command, "@InstitutionId", institutionId, DbType.Int32));

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

        public PagedResult<DocumentEntity> GetPaged(DocumentQuery query)
        {
            var documents = new List<DocumentEntity>();
            int totalCount;

            var allowedTypes = query.AllowedTypes != null && query.AllowedTypes.Count > 0
                ? string.Join(",", query.AllowedTypes.Select(t => (int)t))
                : null;

            try
            {
                using (var connection = CreateConnection())
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Document_GetPaged";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(SetParameter(command, "@InstitutionId", query.InstitutionId, DbType.Int32));
                    command.Parameters.Add(SetParameter(command, "@PageNumber", query.PageNumber, DbType.Int32));
                    command.Parameters.Add(SetParameter(command, "@PageSize", query.PageSize, DbType.Int32));
                    command.Parameters.Add(SetParameter(command, "@AllowedTypes", allowedTypes, DbType.String));
                    command.Parameters.Add(SetParameter(command, "@TypeFilter",
                        query.TypeFilter.HasValue ? (int)query.TypeFilter.Value : (int?)null, DbType.Int32));
                    command.Parameters.Add(SetParameter(command, "@StatusFilter",
                        query.StatusFilter.HasValue ? (int)query.StatusFilter.Value : (int?)null, DbType.Int32));
                    command.Parameters.Add(SetParameter(command, "@SortColumn", query.SortColumn, DbType.AnsiString));
                    command.Parameters.Add(SetParameter(command, "@SortDirection", query.SortDirection, DbType.Boolean));

                    var totalCountParam = SetOutputParameter(command, "@TotalCount", DbType.Int32);
                    command.Parameters.Add(totalCountParam);

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

                    totalCount = totalCountParam.Value != DBNull.Value ? (int)totalCountParam.Value : 0;
                }
            }
            catch (Exception ex)
            {
                var e = ex;
                return new PagedResult<DocumentEntity>();
            }

            return new PagedResult<DocumentEntity>
            {
                Items = documents,
                TotalItems = totalCount
            };
        }

        public int Insert(DocumentEntity document)
        {
            using (var connection = CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "Document_Insert";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(SetParameter(command, "@InstitutionId", document.InstitutionId, DbType.Int32));
                command.Parameters.Add(SetParameter(command, "@Name", document.Name, DbType.AnsiString));
                command.Parameters.Add(SetParameter(command, "@Type", (int)document.Type, DbType.Int32));
                command.Parameters.Add(SetParameter(command, "@UploadDate", document.UploadDate, DbType.DateTime2));
                command.Parameters.Add(SetParameter(command, "@Status", (int)document.Status, DbType.Int32));

                var documentIdParam = SetOutputParameter(command, "@DocumentId", DbType.Int32);
                command.Parameters.Add(documentIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)documentIdParam.Value;
            }
        }
    }
}