using MiniDocumentNotifier.Domain.Models;

namespace MiniDocumentNotifier.Domain.Abstractions
{
    public interface IUserPreferencesStore
    {
        UserPreferences Load();
        void Save(UserPreferences preferences);
    }
}