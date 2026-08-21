using Microsoft.Win32;

namespace MiniDocumentNotifier.Application.RegistryWrapper
{
    public class RegistryStore : IRegistryStore
    {
        public string GetValue(string keyPath, string valueName)
        {
            using var key = Registry.CurrentUser.OpenSubKey(keyPath);
            return key?.GetValue(valueName)?.ToString();
        }

        public void SetValue(string keyPath, string valueName, string value)
        {
            using var key = Registry.CurrentUser.CreateSubKey(keyPath);
            key?.SetValue(valueName, value, RegistryValueKind.String);
        }
    }
}