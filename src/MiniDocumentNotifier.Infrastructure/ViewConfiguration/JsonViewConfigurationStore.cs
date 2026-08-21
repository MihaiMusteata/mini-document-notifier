using System;
using System.Collections.Generic;
using System.Linq;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using Newtonsoft.Json.Linq;

namespace MiniDocumentNotifier.Infrastructure.ViewConfiguration
{
    public class JsonViewConfigurationStore : IViewConfigurationStore
    {
        private readonly TimeSpan _stalenessThreshold;
        private readonly string _path;
        private readonly ILogger _logger;
        private readonly IFileStorage _fileStorage;

        public JsonViewConfigurationStore(TimeSpan stalenessThreshold, string path, ILogger logger, IFileStorage fileStorage)
        {
            _stalenessThreshold = stalenessThreshold;
            _path = path;
            _logger = logger;
            _fileStorage = fileStorage;
        }

        public ViewConfigurationResult Load()
        {
            if (!_fileStorage.Exists(_path))
            {
                _logger.Warning($"View configuration file not found at '{_path}'.");
                return new ViewConfigurationResult
                {
                    FileExists = false,
                    IsStale = false,
                    Institutions = new List<InstitutionViewConfiguration>()
                };
            }

            var lastWrite = _fileStorage.GetLastWriteTimeUtc(_path);
            var isStale = DateTime.UtcNow - lastWrite > _stalenessThreshold;

            if (isStale)
            {
                _logger.Warning($"View configuration file '{_path}' is stale: last written {lastWrite:O} UTC, staleness threshold {_stalenessThreshold}.");
            }

            JArray root;
            try
            {
                root = JArray.Parse(_fileStorage.ReadAllText(_path));
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to parse view configuration file '{_path}'.", ex);
                return new ViewConfigurationResult
                {
                    FileExists = true,
                    IsStale = isStale,
                    Institutions = new List<InstitutionViewConfiguration>()
                };
            }

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