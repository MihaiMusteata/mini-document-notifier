using System.Collections.Generic;
using MiniDocumentNotifier.WinForms.Models;

namespace MiniDocumentNotifier.WinForms.Views
{
    public interface IMainView
    {
        string SearchText { get; }
        string SelectedTypeFilterLabel { get; }
        string SelectedStatusFilterLabel { get; }

        void DisplayDocuments(List<DocumentRow> rows, IReadOnlyDictionary<string, float> columnWidths);

        List<DocumentRow> Rows { set; }
        void SetTypeFilterOptions(List<string> options);
        void SetStatusFilterOptions(List<string> options);
        void SetPagingInfo(string pageText, bool prevEnabled, bool nextEnabled);
        void ApplyColumnWidth(string columnName, float weight);

        void ShowWarning(string message);
        void HideWarning();
        void ShowTimeoutError();

        void SetSortIndicator(string columnName, bool descending);
    }
}