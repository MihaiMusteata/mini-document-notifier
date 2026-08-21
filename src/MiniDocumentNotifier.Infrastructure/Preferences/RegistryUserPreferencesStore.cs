using System;
using MiniDocumentNotifier.Application.RegistryWrapper;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using Newtonsoft.Json;

namespace MiniDocumentNotifier.Infrastructure.Preferences
{
    public class RegistryUserPreferencesStore : IUserPreferencesStore
    {
        private readonly string _registryKeyPath;
        private readonly ILogger _logger;
        private readonly IRegistryStore _registryStore;

        public RegistryUserPreferencesStore(string registryKeyPath, ILogger logger, IRegistryStore registryStore)
        {
            _registryKeyPath = registryKeyPath;
            _logger = logger;
            _registryStore = registryStore;
        }

        
        public UserPreferences Load()
        {
            try
            {
                var json = _registryStore.GetValue(_registryKeyPath, "UserPreferences");

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
                var json = JsonConvert.SerializeObject(preferences);
                _registryStore.SetValue(_registryKeyPath, "UserPreferences", json);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to write user preferences to registry key '{_registryKeyPath}' (source: Registry).", ex);
            }
        }
    }
}
