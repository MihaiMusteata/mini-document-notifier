using System.IO;
using MiniDocumentNotifier.Domain.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniDocumentNotifier.Application.Sync
{
    public class ViewConfigurationSyncService : IViewConfigurationSyncService
    {
        private readonly IViewConfigurationSyncServiceClient _client;
        private readonly string _outputFilePath;
        private readonly ILogger _logger;

        public ViewConfigurationSyncService(IViewConfigurationSyncServiceClient client, string outputFilePath,
            ILogger logger)
        {
            _client = client;
            _outputFilePath = outputFilePath;
            _logger = logger;
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

            _logger.Info($"View configuration sync cycle completed: {viewConfigurations.Count} institution(s) synced, written to '{_outputFilePath}'.");
        }
    }
}