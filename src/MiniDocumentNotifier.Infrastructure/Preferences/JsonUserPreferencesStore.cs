using System;
using System.IO;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using Newtonsoft.Json;

namespace MiniDocumentNotifier.Infrastructure.Preferences
{
    public class JsonUserPreferencesStore : IUserPreferencesStore
    {
        private readonly string _filePath;
        private readonly ILogger _logger;

        public JsonUserPreferencesStore(string filePath, ILogger logger)
        {
            _filePath = filePath;
            _logger = logger;
        }

        public UserPreferences Load()
        {
            if (!File.Exists(_filePath))
            {
                var defaults = UserPreferences.CreateDefault();
                _logger.Info($"User preferences file not found, created defaults at '{_filePath}' (source: JSON).");
                Save(defaults);
                return defaults;
            }

            try
            {
                var json = File.ReadAllText(_filePath);
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
                var dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(_filePath, JsonConvert.SerializeObject(preferences, Formatting.Indented));
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to write user preferences to '{_filePath}' (source: JSON).", ex);
            }
        }
    }
}
