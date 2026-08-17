using Microsoft.Win32;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using Newtonsoft.Json;

namespace MiniDocumentNotifier.Infrastructure.Preferences
{
    public class RegistryUserPreferencesStore : IUserPreferencesStore
    {
        private readonly string _registryKeyPath;

        public RegistryUserPreferencesStore(string registryKeyPath)
        {
            _registryKeyPath = registryKeyPath;
        }

        public UserPreferences Load()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(_registryKeyPath))
            {
                var json = key?.GetValue("UserPreferences")?.ToString();

                if (json != null && !string.IsNullOrEmpty(json))
                    return JsonConvert.DeserializeObject<UserPreferences>(json) ?? UserPreferences.CreateDefault();

                var defaults = UserPreferences.CreateDefault();
                Save(defaults);
                return defaults;
            }
        }

        public void Save(UserPreferences preferences)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(_registryKeyPath))
            {
                var json = JsonConvert.SerializeObject(preferences);
                key?.SetValue("UserPreferences", json, RegistryValueKind.String);
            }
        }
    }
}