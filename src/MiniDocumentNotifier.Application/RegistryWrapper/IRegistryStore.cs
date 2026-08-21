namespace MiniDocumentNotifier.Application.RegistryWrapper
{
    public interface IRegistryStore
    {
        string GetValue(string keyPath, string valueName);
        void SetValue(string keyPath, string valueName, string value);
    }
}