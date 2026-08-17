using System.Collections.Generic;

namespace MiniDocumentNotifier.Domain.Models
{
    public class UserPreferences
    {
        public string DefaultSortColumn { get; set; }
        public bool DefaultSortDirection { get; set; }
        public string LastUsername { get; set; }
        public Dictionary<string, float> ColumnWidths { get; set; }

        public static UserPreferences CreateDefault()
        {
            return new UserPreferences
            {
                DefaultSortColumn = "UploadDate",
                DefaultSortDirection = true,
                LastUsername = null,
                ColumnWidths = new Dictionary<string, float>()
            };
        }
    }
}