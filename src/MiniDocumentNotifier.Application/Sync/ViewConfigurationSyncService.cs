using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniDocumentNotifier.Application.Sync
{
    public class ViewConfigurationSyncService : IViewConfigurationSyncService
    {
        private readonly IViewConfigurationSyncServiceClient _client;
        private readonly string _outputFilePath;

        public ViewConfigurationSyncService(IViewConfigurationSyncServiceClient client, string outputFilePath)
        {
            _client = client;
            _outputFilePath = outputFilePath;
        }

        public void SyncAll()
        {
            var root = new JArray();
            var viewConfigurations = _client.GetAllViewConfigurations();

            foreach (var configuration in viewConfigurations)
            {
                root.Add(new JObject
                {
                    ["institutionId"] = configuration.InstitutionId,
                    ["institutionCode"] = configuration.InstitutionCode,
                    ["visibleColumns"] = JToken.Parse(configuration.VisibleColumns),
                    ["activeCategories"] = JToken.Parse(configuration.ActiveCategories),
                    ["lastUpdatedDate"] = configuration.LastUpdatedDate
                });
            }

            File.WriteAllText(_outputFilePath, root.ToString(Formatting.Indented));
        }
    }
}