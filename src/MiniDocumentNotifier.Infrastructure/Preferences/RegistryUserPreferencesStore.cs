using System;
using Microsoft.Win32;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using Newtonsoft.Json;

namespace MiniDocumentNotifier.Infrastructure.Preferences
{
    public class RegistryUserPreferencesStore : IUserPreferencesStore
    {
        private readonly string _registryKeyPath;
        private readonly ILogger _logger;

        public RegistryUserPreferencesStore(string registryKeyPath, ILogger logger)
        {
            _registryKeyPath = registryKeyPath;
            _logger = logger;
        }

        public UserPreferences Load()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(_registryKeyPath))
                {
                    var json = key?.GetValue("UserPreferences")?.ToString();

                    if (!string.IsNullOrEmpty(json))
                    {
                        var preferences = JsonConvert.DeserializeObject<UserPreferences>(json) ?? UserPreferences.CreateDefault();
                        _logger.Info($"User preferences loaded from registry key '{_registryKeyPath}' (source: Registry).");
                        return preferences;
                    }

                    var defaults = UserPreferences.CreateDefault();
                    _logger.Info($"User preferences registry value not found; created defaults at '{_registryKeyPath}' (source: Registry).");
                    Save(defaults);
                    return defaults;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to read user preferences from registry key '{_registryKeyPath}' (source: Registry).", ex);
                return UserPreferences.CreateDefault();
            }
        }

        public void Save(UserPreferences preferences)
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey(_registryKeyPath))
                {
                    var json = JsonConvert.SerializeObject(preferences);
                    key?.SetValue("UserPreferences", json, RegistryValueKind.String);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to write user preferences to registry key '{_registryKeyPath}' (source: Registry).", ex);
            }
        }
    }
}
