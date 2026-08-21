using System;
using System.IO;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using Newtonsoft.Json;

namespace MiniDocumentNotifier.Infrastructure.Preferences
{
    public class JsonUserPreferencesStore : IUserPreferencesStore
    {
        private readonly string _filePath;
        private readonly ILogger _logger;
        private readonly IFileStorage _fileStorage;

        public JsonUserPreferencesStore(string filePath, ILogger logger, IFileStorage fileStorage)
        {
            _filePath = filePath;
            _logger = logger;
            _fileStorage = fileStorage;
        }

        public UserPreferences Load()
        {
            if (!_fileStorage.Exists(_filePath))
            {
                var defaults = UserPreferences.CreateDefault();
                _logger.Info($"User preferences file not found, created defaults at '{_filePath}' (source: JSON).");
                Save(defaults);
                return defaults;
            }

            try
            {
                var json = _fileStorage.ReadAllText(_filePath);
                var preferences = JsonConvert.DeserializeObject<UserPreferences>(json) ?? UserPreferences.CreateDefault();
                _logger.Info($"User preferences loaded from '{_filePath}' (source: JSON).");
                return preferences;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to read user preferences from '{_filePath}' (source: JSON).", ex);
                return UserPreferences.CreateDefault();
            }
        }

        public void Save(UserPreferences preferences)
        {
            try
            {
                var dir = _fileStorage.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir) && !_fileStorage.DirectoryExists(dir))
                {
                    _fileStorage.CreateDirectory(dir);
                }

                _fileStorage.WriteAllText(_filePath, JsonConvert.SerializeObject(preferences, Formatting.Indented));
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to write user preferences to '{_filePath}' (source: JSON).", ex);
            }
        }
    }
}
