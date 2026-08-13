using System.IO;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using Newtonsoft.Json;

namespace MiniDocumentNotifier.Infrastructure.Preferences
{
    public class JsonUserPreferencesStore : IUserPreferencesStore
    {
        private readonly string _filePath;

        public JsonUserPreferencesStore(string filePath)
        {
            _filePath = filePath;
        }

        public UserPreferences Load()
        {
            if (!File.Exists(_filePath))
            {
                var defaults = UserPreferences.CreateDefault();
                Save(defaults);
                return defaults;
            }

            // just for checking if UI doesn't freeze during this call
            // Thread.Sleep(2000);
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<UserPreferences>(json) ?? UserPreferences.CreateDefault();
        }

        public void Save(UserPreferences preferences)
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(preferences, Formatting.Indented));
        }
    }
}