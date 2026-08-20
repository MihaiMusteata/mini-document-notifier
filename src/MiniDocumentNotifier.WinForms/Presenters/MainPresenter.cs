using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Infrastructure.ServiceClient;
using MiniDocumentNotifier.WinForms.Forms;
using MiniDocumentNotifier.WinForms.Models;
using MiniDocumentNotifier.WinForms.Services;
using MiniDocumentNotifier.WinForms.Views;

namespace MiniDocumentNotifier.WinForms.Presenters
{
    public class MainPresenter
    {
        private const int PageSize = 20;

        private readonly IMainView _view;
        private readonly IUserPreferencesStore _preferencesStore;
        private readonly IViewConfigurationStore _viewConfigurationStore;
        private readonly IDocumentNotifierServiceClient _serviceClient;
        private readonly bool _isBackgroundAppRunning;
        private readonly int _institutionId;

        private readonly DocumentGridState _gridState = new DocumentGridState();
        private UserPreferences _preferences;
        private InstitutionViewConfiguration _institutionConfiguration;

        public MainPresenter(
            IMainView view,
            IUserPreferencesStore preferencesStore,
            IViewConfigurationStore viewConfigurationStore,
            IDocumentNotifierServiceClient serviceClient,
            bool isBackgroundAppRunning,
            int institutionId)
        {
            _view = view;
            _preferencesStore = preferencesStore;
            _viewConfigurationStore = viewConfigurationStore;
            _serviceClient = serviceClient;
            _isBackgroundAppRunning = isBackgroundAppRunning;
            _institutionId = institutionId;
        }


        #region Initialization & Configuration

        public async Task InitializeAsync()
        {
            _preferences = _preferencesStore.Load();
            CheckConfiguration();
            PopulateFilterOptions();
            await LoadPageAsync();
        }

        public void CheckConfiguration()
        {
            var result = _viewConfigurationStore.Load();

            if (!result.FileExists || result.IsStale || !_isBackgroundAppRunning)
            {
                var message = !result.FileExists ? "Configuration file not found." :
                    result.IsStale ? "Configuration file is stale." : "";

                if (!_isBackgroundAppRunning)
                    message += " Background App is not running.";

                _view.ShowWarning(message);
                return;
            }

            _institutionConfiguration = result.Institutions?.FirstOrDefault(i => i.InstitutionId == _institutionId);

            if (_institutionConfiguration == null)
            {
                _view.ShowWarning("No view configuration found for this institution. Showing all documents.");
            }
        }

        public void PopulateFilterOptions()
        {
            var allowedTypes = GetAllowedTypes() ?? Enum.GetValues(typeof(DocumentType)).Cast<DocumentType>().ToList();

            var typeOptions = new List<string> { "All" };
            typeOptions.AddRange(allowedTypes.Select(t => DocumentLabels.DocumentTypeLabels[t]));
            _view.SetTypeFilterOptions(typeOptions);

            var statusOptions = new List<string> { "All" };
            statusOptions.AddRange(Enum.GetValues(typeof(DocumentStatus)).Cast<DocumentStatus>()
                .Select(s => DocumentLabels.DocumentStatusLabels[s]));
            _view.SetStatusFilterOptions(statusOptions);
        }

        public List<DocumentType> GetAllowedTypes()
        {
            if (_institutionConfiguration == null) return null;

            return Newtonsoft.Json.JsonConvert
                .DeserializeObject<List<string>>(_institutionConfiguration.ActiveCategories)
                .Select(t => (DocumentType)Enum.Parse(typeof(DocumentType), t))
                .ToList();
        }

        #endregion

        #region Data Loading

        public async Task LoadPageAsync()
        {
            try
            {
                var request = new DocumentQueryRequest
                {
                    InstitutionId = _institutionId,
                    PageNumber = _gridState.CurrentPage,
                    PageSize = PageSize,
                    AllowedTypes = GetAllowedTypes(),
                    TypeFilter = _gridState.TypeFilter,
                    StatusFilter = _gridState.StatusFilter,
                    SortColumn = _preferences.DefaultSortColumn,
                    SortDirection = _preferences.DefaultSortDirection
                };

                var result = await Task.Run(() => _serviceClient.GetDocumentsPaged(request));

                _gridState.TotalCount = result.Total;

                var searchTerm = _view.SearchText?.Trim();
                var documents = result.Documents.AsEnumerable();

                if (!string.IsNullOrEmpty(searchTerm))
                    documents = documents.Where(d =>
                        d.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);

                var rows = documents.Select(d => new DocumentRow
                {
                    Id = d.Id,
                    Name = d.Name,
                    Type = DocumentLabels.DocumentTypeLabels[d.Type],
                    Status = DocumentLabels.DocumentStatusLabels[d.Status],
                    UploadDate = d.UploadDate
                }).ToList();

                _view.DisplayDocuments(rows, _preferences.ColumnWidths);

                UpdatePagingInfo();
            }
            catch (EndpointNotFoundException)
            {
                _view.ShowWarning("Service is not available.");
            }
            catch (CommunicationException)
            {
                _view.ShowWarning("Communication error with the service.");
            }
            catch (TimeoutException)
            {
                _view.ShowTimeoutError();
            }
        }

        public void UpdatePagingInfo()
        {
            var totalPages = (int)Math.Ceiling(_gridState.TotalCount / (double)PageSize);
            var pageText = $"Page {_gridState.CurrentPage + 1} of {Math.Max(totalPages, 1)}";
            var prevEnabled = _gridState.CurrentPage > 0;
            var nextEnabled = (_gridState.CurrentPage + 1) * PageSize < _gridState.TotalCount;

            _view.SetPagingInfo(pageText, prevEnabled, nextEnabled);
        }

        #endregion

        #region View Event Handlers

        public async Task OnNextPageAsync()
        {
            _gridState.CurrentPage++;
            await LoadPageAsync();
        }

        public async Task OnPrevPageAsync()
        {
            if (_gridState.CurrentPage == 0) return;
            _gridState.CurrentPage--;
            await LoadPageAsync();
        }

        public async Task OnColumnHeaderClickedAsync(string columnName)
        {
            var descending = _preferences.DefaultSortColumn == columnName && !_preferences.DefaultSortDirection;

            _preferences.DefaultSortColumn = columnName;
            _preferences.DefaultSortDirection = descending;

            _view.SetSortIndicator(columnName, descending);

            await LoadPageAsync();
        }

        public void OnColumnWidthChanged(string columnName, float weight)
        {
            _preferences.ColumnWidths[columnName] = weight;
        }

        public async Task OnTypeFilterChangedAsync(string selectedLabel)
        {
            _gridState.TypeFilter = selectedLabel == "All" || selectedLabel == null
                ? (DocumentType?)null
                : DocumentLabels.DocumentTypeLabels.FirstOrDefault(kv => kv.Value == selectedLabel).Key;

            _gridState.CurrentPage = 0;
            await LoadPageAsync();
        }

        public async Task OnStatusFilterChangedAsync(string selectedLabel)
        {
            _gridState.StatusFilter = selectedLabel == "All" || selectedLabel == null
                ? (DocumentStatus?)null
                : DocumentLabels.DocumentStatusLabels.FirstOrDefault(kv => kv.Value == selectedLabel).Key;

            _gridState.CurrentPage = 0;
            await LoadPageAsync();
        }

        public async Task OnSearchDebounceElapsedAsync()
        {
            _gridState.CurrentPage = 0;
            await LoadPageAsync();
        }

        public void OnDismissWarning()
        {
            _view.HideWarning();
        }

        public async Task OnUploadCompletedAsync()
        {
            await LoadPageAsync();
        }

        public void OnClosing()
        {
            _preferencesStore.Save(_preferences);
        }

        #endregion

        #region Properties

        public int InstitutionId => _institutionId;

        #endregion
    }
}