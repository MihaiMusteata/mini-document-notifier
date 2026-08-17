using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Domain.Models;
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

        public PagedResult<DocumentEntity> GetPaged(DocumentQuery query)
        {
            var documents = new List<DocumentEntity>();
            int totalCount;

            var allowedTypes = query.AllowedTypes != null && query.AllowedTypes.Count > 0
                ? string.Join(",", query.AllowedTypes.Select(t => (int)t))
                : null;

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                using (var command = new SqlCommand("Document_GetPaged", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(SetParameter("@InstitutionId", query.InstitutionId, SqlDbType.Int));
                    command.Parameters.Add(SetParameter("@PageNumber", query.PageNumber, SqlDbType.Int));
                    command.Parameters.Add(SetParameter("@PageSize", query.PageSize, SqlDbType.Int));
                    command.Parameters.Add(SetParameter("@AllowedTypes", allowedTypes, SqlDbType.NVarChar));
                    command.Parameters.Add(SetParameter("@TypeFilter",
                        query.TypeFilter.HasValue ? (int)query.TypeFilter.Value : (int?)null, SqlDbType.Int));
                    command.Parameters.Add(SetParameter("@StatusFilter",
                        query.StatusFilter.HasValue ? (int)query.StatusFilter.Value : (int?)null, SqlDbType.Int));
                    command.Parameters.Add(SetParameter("@SortColumn", query.SortColumn, SqlDbType.VarChar));
                    command.Parameters.Add(SetParameter("@SortDirection", query.SortDirection, SqlDbType.Bit));

                    var totalCountParam = SetOutputParameter("@TotalCount", SqlDbType.Int);
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
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand("Document_Insert", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(SetParameter("@InstitutionId", document.InstitutionId, SqlDbType.Int));
                command.Parameters.Add(SetParameter("@Name", document.Name, SqlDbType.VarChar));
                command.Parameters.Add(SetParameter("@Type", (int)document.Type, SqlDbType.Int));
                command.Parameters.Add(SetParameter("@UploadDate", document.UploadDate, SqlDbType.DateTime2));
                command.Parameters.Add(SetParameter("@Status", (int)document.Status, SqlDbType.Int));

                var documentIdParam = SetOutputParameter("@DocumentId", SqlDbType.Int);
                command.Parameters.Add(documentIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)documentIdParam.Value;
            }
        }
    }
}