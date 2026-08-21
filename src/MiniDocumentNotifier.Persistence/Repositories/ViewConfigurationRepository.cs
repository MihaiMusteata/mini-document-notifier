using System;
using System.Collections.Generic;
using System.Data;
using MiniDocumentNotifier.Domain.Entities;
using MiniDocumentNotifier.Domain.Repositories;
using MiniDocumentNotifier.Persistence.SqlConnFactory;

namespace MiniDocumentNotifier.Persistence.Repositories
{
    public class ViewConfigurationRepository : BaseRepository, IViewConfigurationRepository
    {
        public ViewConfigurationRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public List<ViewConfigurationEntity> GetAllWithInstitutions()
        {
            var viewConfigurations = new List<ViewConfigurationEntity>();

            using (var connection = CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "dbo.ViewConfiguration_GetAllWithInstitutions";
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var institution = new InstitutionEntity
                        {
                            Id = (int)reader["Institution_Id"],
                            Code = (string)reader["Institution_Code"],
                            Name = (string)reader["Institution_Name"]
                        };

                        viewConfigurations.Add(new ViewConfigurationEntity
                        {
                            Id = (int)reader["Id"],
                            InstitutionId = (int)reader["InstitutionId"],
                            VisibleColumns = (string)reader["VisibleColumns"],
                            ActiveCategories = (string)reader["ActiveCategories"],
                            LastUpdatedDate = (DateTime)reader["LastUpdatedDate"],
                            Institution = institution
                        });
                    }
                }
            }
            
            return viewConfigurations;
        }
    }
}