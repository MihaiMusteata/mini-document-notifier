using System.IO;
using MiniDocumentNotifier.Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniDocumentNotifier.Application.Sync
{
    public class ViewConfigurationSyncService : IViewConfigurationSyncService
    {
        private readonly IViewConfigurationRepository _viewConfigurationRepository;
        private readonly string _outputFilePath;

        public ViewConfigurationSyncService(IViewConfigurationRepository viewConfigurationRepository, string outputFilePath)
        {
            _viewConfigurationRepository = viewConfigurationRepository;
            _outputFilePath = outputFilePath;
        }

        public void SyncAll()
        {
            var root = new JArray();
            var viewConfigurations = _viewConfigurationRepository.GetAllWithInstitutions();

            foreach (var configuration in viewConfigurations)
            {
                root.Add(new JObject
                {
                    ["institutionId"] = configuration.Institution.Id,
                    ["institutionCode"] = configuration.Institution.Code,
                    ["visibleColumns"] = JToken.Parse(configuration.VisibleColumns),
                    ["activeCategories"] = JToken.Parse(configuration.ActiveCategories),
                    ["lastUpdatedDate"] = configuration.LastUpdatedDate
                });
            }

            File.WriteAllText(_outputFilePath, root.ToString(Formatting.Indented));
        }
    }
}