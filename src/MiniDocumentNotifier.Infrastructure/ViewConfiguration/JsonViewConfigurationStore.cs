using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using Newtonsoft.Json.Linq;

namespace MiniDocumentNotifier.Infrastructure.ViewConfiguration
{
    public class JsonViewConfigurationStore : IViewConfigurationStore
    {
        private readonly TimeSpan _stalenessThreshold;
        private readonly string _path;

        public JsonViewConfigurationStore(TimeSpan stalenessThreshold, string path)
        {
            _stalenessThreshold = stalenessThreshold;
            _path = path;
        }

        public ViewConfigurationResult Load()
        {
            if (!File.Exists(_path))
                return new ViewConfigurationResult
                {
                    FileExists = false,
                    IsStale = false,
                    Institutions = new List<InstitutionViewConfiguration>()
                };

            var lastWrite = File.GetLastWriteTimeUtc(_path);
            var isStale = DateTime.UtcNow - lastWrite > _stalenessThreshold;

            var root = JArray.Parse(File.ReadAllText(_path));

            var institutions = root.Select(node => new InstitutionViewConfiguration
            {
                InstitutionId = node["institutionId"].Value<int>(),
                VisibleColumns = node["visibleColumns"].ToString(),
                ActiveCategories = node["activeCategories"].ToString()
            }).ToList();

            return new ViewConfigurationResult { FileExists = true, IsStale = isStale, Institutions = institutions };
        }
    }
}