using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniDocumentNotifier.Contracts.DocumentContracts;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Enums;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Infrastructure.ServiceClient;
using MiniDocumentNotifier.WinForms.Models;
using MiniDocumentNotifier.WinForms.Presenters;
using MiniDocumentNotifier.WinForms.Services;
using MiniDocumentNotifier.WinForms.Views;
using Moq;

namespace MiniDocumentNotifier.WinForms.Tests.Presenters
{
    [TestClass]
    public class MainPresenterTest
    {
        private Fixture _fixture;
        private Mock<IMainView> _view;
        private Mock<IUserPreferencesStore> _preferencesStore;
        private Mock<IViewConfigurationStore> _viewConfigurationStore;
        private Mock<IDocumentNotifierServiceClient> _client;
        private UserPreferences _preferences;

        private MainPresenter _presenter;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();

            _preferences = new UserPreferences { ColumnWidths = new Dictionary<string, float>() };

            _view = new Mock<IMainView>();
            _view.SetupGet(v => v.SearchText).Returns(string.Empty);

            _preferencesStore = new Mock<IUserPreferencesStore>();
            _preferencesStore.Setup(s => s.Load()).Returns(_preferences);

            _viewConfigurationStore = new Mock<IViewConfigurationStore>();
            _viewConfigurationStore.Setup(s => s.Load()).Returns(new ViewConfigurationResult
            {
                FileExists = true,
                IsStale = false,
                Institutions = new List<InstitutionViewConfiguration>()
            });

            _client = new Mock<IDocumentNotifierServiceClient>();
            _client
                .Setup(s => s.GetDocumentsPaged(It.IsAny<DocumentQueryRequest>()))
                .Returns(new DocumentQueryResult { Total = 45, Documents = new List<DocumentDto>() });

            _presenter = new MainPresenter(
                _view.Object,
                _preferencesStore.Object,
                _viewConfigurationStore.Object,
                _client.Object,
                isBackgroundAppRunning: true,
                institutionId: 1);
        }

        private async Task InitializePresenterAsync()
        {
            await _presenter.InitializeAsync();
            _view.Invocations.Clear();
            _client.Invocations.Clear();
        }

        [TestMethod]
        public void CheckConfiguration_FileMissing_ShowsCorrectWarning()
        {
            _viewConfigurationStore
                .Setup(s => s.Load())
                .Returns(new ViewConfigurationResult { FileExists = false });

            _presenter.CheckConfiguration();

            _view.Verify(v => v.ShowWarning("Configuration file not found."), Times.Once);
        }

        [TestMethod]
        public void CheckConfiguration_FileStale_ShowsCorrectWarning()
        {
            _viewConfigurationStore.Setup(s => s.Load()).Returns(new ViewConfigurationResult
            {
                FileExists = true,
                IsStale = true
            });

            _presenter.CheckConfiguration();

            _view.Verify(v => v.ShowWarning("Configuration file is stale."), Times.Once);
        }

        [TestMethod]
        public void CheckConfiguration_BackgroundAppNotRunning_ShowsCorrectWarning()
        {
            _viewConfigurationStore.Setup(s => s.Load()).Returns(new ViewConfigurationResult
            {
                FileExists = true,
                IsStale = false
            });
            var mainPresenter = new MainPresenter(_view.Object, _preferencesStore.Object,
                _viewConfigurationStore.Object, _client.Object, isBackgroundAppRunning: false, institutionId: 1);

            mainPresenter.CheckConfiguration();

            _view.Verify(v => v.ShowWarning(" Background App is not running."), Times.Once);
        }

        [TestMethod]
        public void CheckConfiguration_ValidConfigButInstitutionMissing_ShowsWarning()
        {
            _viewConfigurationStore.Setup(s => s.Load()).Returns(new ViewConfigurationResult
            {
                FileExists = true,
                IsStale = false,
                Institutions = new List<InstitutionViewConfiguration>()
            });

            _presenter.CheckConfiguration();

            _view.Verify(v => v.ShowWarning("No view configuration found for this institution. Showing all documents."),
                Times.Once);
        }

        [TestMethod]
        public void CheckConfiguration_ValidConfigAndInstitutionFound_DoesNotShowWarning()
        {
            _viewConfigurationStore.Setup(s => s.Load()).Returns(new ViewConfigurationResult
            {
                FileExists = true,
                IsStale = false,
                Institutions = new List<InstitutionViewConfiguration>
                {
                    new InstitutionViewConfiguration
                    {
                        InstitutionId = 1,
                        ActiveCategories = "[\"Statement\"]"
                    }
                }
            });

            _presenter.CheckConfiguration();

            _view.Verify(v => v.ShowWarning(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task InitializeAsync_LoadsPreferencesConfigFiltersAndData()
        {
            await _presenter.InitializeAsync();

            _preferencesStore.Verify(s => s.Load(), Times.Once);
            _view.Verify(v => v.SetTypeFilterOptions(It.IsAny<List<string>>()), Times.Once);
            _view.Verify(v => v.SetStatusFilterOptions(It.IsAny<List<string>>()), Times.Once);
            _client.Verify(s => s.GetDocumentsPaged(It.IsAny<DocumentQueryRequest>()), Times.Once);
        }

        [TestMethod]
        public void GetAllowedTypes_NoInstitutionConfiguration_ReturnsNull()
        {
            _viewConfigurationStore.Setup(s => s.Load()).Returns(new ViewConfigurationResult { FileExists = false });
            _presenter.CheckConfiguration();

            var result = _presenter.GetAllowedTypes();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllowedTypes_ValidJsonInConfiguration_ReturnsParsedTypes()
        {
            _viewConfigurationStore.Setup(s => s.Load()).Returns(new ViewConfigurationResult
            {
                FileExists = true,
                IsStale = false,
                Institutions = new List<InstitutionViewConfiguration>
                {
                    new InstitutionViewConfiguration
                    {
                        InstitutionId = 1,
                        ActiveCategories = "[\"Statement\",\"Contract\"]"
                    }
                }
            });
            _presenter.CheckConfiguration();

            var result = _presenter.GetAllowedTypes();

            CollectionAssert.AreEqual(new[] { DocumentType.Statement, DocumentType.Contract }, result);
        }

        #region Data Loading

        [TestMethod]
        [DataRow(0)]
        [DataRow(3)]
        [DataRow(10)]
        [DataRow(23)]
        [DataRow(60)]
        [DataRow(90)]
        public async Task LoadPageAsync_Success_DisplaysDocumentsAndUpdatesPaging(int totalItems)
        {
            const int pageSize = 20;
            await InitializePresenterAsync();

            var docs = _fixture.CreateMany<DocumentDto>(totalItems).ToList();
            _client.Setup(c => c.GetDocumentsPaged(It.IsAny<DocumentQueryRequest>()))
                .Returns(new DocumentQueryResult { Total = totalItems, Documents = docs });

            await _presenter.LoadPageAsync();

            _view.Verify(v => v.DisplayDocuments(
                It.Is<List<DocumentRow>>(rows => rows.Count == totalItems),
                It.IsAny<IReadOnlyDictionary<string, float>>()), Times.Once);

            var totalPages = Math.Max((int)Math.Ceiling(totalItems / (double)pageSize), 1);

            _view.Verify(v => v.SetPagingInfo($"Page 1 of {totalPages}", false, totalPages > 1), Times.Once);
        }

        [TestMethod]
        public async Task LoadPageAsync_WithSearchTerm_FiltersDocumentsByName()
        {
            await InitializePresenterAsync();

            _view.SetupGet(v => v.SearchText).Returns("Invoice");

            var docs = new List<DocumentDto>
            {
                new DocumentDto
                    { Id = 1, Name = "Invoice January", Type = DocumentType.Statement, Status = DocumentStatus.New },
                new DocumentDto
                    { Id = 2, Name = "Contract renewal", Type = DocumentType.Contract, Status = DocumentStatus.New }
            };

            _client.Setup(c => c.GetDocumentsPaged(It.IsAny<DocumentQueryRequest>()))
                .Returns(new DocumentQueryResult { Total = 2, Documents = docs });

            await _presenter.LoadPageAsync();

            _view.Verify(v => v.DisplayDocuments(
                It.Is<List<DocumentRow>>(rows => rows.Count == 1 && rows[0].Name == "Invoice January"),
                It.IsAny<IReadOnlyDictionary<string, float>>()), Times.Once);
        }

        [TestMethod]
        [DataRow(typeof(EndpointNotFoundException), "Service is not available.")]
        [DataRow(typeof(CommunicationException), "Communication error with the service.")]
        [DataRow(typeof(TimeoutException), null)]
        public async Task LoadPageAsync_ServiceException_ShowsExpectedError(Type exceptionType, string expectedWarning)
        {
            await InitializePresenterAsync();

            _client.Setup(c => c.GetDocumentsPaged(It.IsAny<DocumentQueryRequest>()))
                .Throws((Exception)Activator.CreateInstance(exceptionType));

            await _presenter.LoadPageAsync();

            if (expectedWarning != null)
                _view.Verify(v => v.ShowWarning(expectedWarning), Times.Once);
            else
                _view.Verify(v => v.ShowTimeoutError(), Times.Once);
        }

        #endregion


        #region View Event Handlers

        [TestMethod]
        public async Task OnNextPageAsync_IncrementsPageAndReloadsData()
        {
            await InitializePresenterAsync();

            await _presenter.OnNextPageAsync();

            _client.Verify(c => c.GetDocumentsPaged(
                It.Is<DocumentQueryRequest>(r => r.PageNumber == 1)), Times.Once);
        }

        [TestMethod]
        public async Task OnPrevPageAsync_DecrementsPageAndReloadsData()
        {
            await InitializePresenterAsync();
            await _presenter.OnNextPageAsync();
            _client.Invocations.Clear();

            await _presenter.OnPrevPageAsync();

            _client.Verify(c => c.GetDocumentsPaged(
                It.Is<DocumentQueryRequest>(r => r.PageNumber == 0)), Times.Once);
        }

        [TestMethod]
        [DataRow("Name", false, "Name", true)]
        [DataRow("Name", true, "Name", false)]
        [DataRow("Name", false, "UploadDate", false)]
        [DataRow("UploadDate", true, "Status", false)]
        [DataRow("Status", true, "Type", false)]
        public async Task OnColumnHeaderClickedAsync_SetsExpectedSort_ReloadsDataAfterSortChange(
            string initialColumn,
            bool initialDirection,
            string clickedColumn,
            bool expectedDirection)
        {
            await InitializePresenterAsync();

            _preferences.DefaultSortColumn = initialColumn;
            _preferences.DefaultSortDirection = initialDirection;

            await _presenter.OnColumnHeaderClickedAsync(clickedColumn);

            Assert.AreEqual(clickedColumn, _preferences.DefaultSortColumn);
            Assert.AreEqual(expectedDirection, _preferences.DefaultSortDirection);

            _view.Verify(v => v.SetSortIndicator(clickedColumn, expectedDirection), Times.Once);
            _client.Verify(c => c.GetDocumentsPaged(It.IsAny<DocumentQueryRequest>()), Times.Once);
        }

        [TestMethod]
        [DataRow("Contract", DocumentType.Contract)]
        [DataRow("All", null)]
        [DataRow(null, null)]
        public async Task OnTypeFilterChangedAsync_SetsExpectedFilter(
            string selectedLabel,
            DocumentType? expectedFilter)
        {
            await InitializePresenterAsync();

            await _presenter.OnNextPageAsync();
            _client.Invocations.Clear();

            await _presenter.OnTypeFilterChangedAsync(selectedLabel);

            _client.Verify(c => c.GetDocumentsPaged(
                    It.Is<DocumentQueryRequest>(r =>
                        r.PageNumber == 0 &&
                        r.TypeFilter == expectedFilter)),
                Times.Once);
        }

        [TestMethod]
        [DataRow("Read", DocumentStatus.Read)]
        [DataRow("All", null)]
        [DataRow(null, null)]
        public async Task OnStatusFilterChangedAsync_SetsExpectedFilter(
            string selectedLabel,
            DocumentStatus? expectedFilter)
        {
            await InitializePresenterAsync();

            await _presenter.OnNextPageAsync();
            _client.Invocations.Clear();

            await _presenter.OnStatusFilterChangedAsync(selectedLabel);

            _client.Verify(c => c.GetDocumentsPaged(
                    It.Is<DocumentQueryRequest>(r =>
                        r.PageNumber == 0 &&
                        r.StatusFilter == expectedFilter)),
                Times.Once);
        }

        [TestMethod]
        [DataRow("Name", 0.50f)]
        [DataRow("Type", 0.30f)]
        public async Task OnColumnWidthChanged_UpdatesColumnWidthInPreferences(string columnName, float newColumnWidth)
        {
            await InitializePresenterAsync();

            _presenter.OnColumnWidthChanged(columnName, newColumnWidth);

            Assert.AreEqual(newColumnWidth, _preferences.ColumnWidths[columnName]);
        }

        [TestMethod]
        public async Task OnSearchDebounceElapsedAsync_ResetPageAndReloadData()
        {
            await InitializePresenterAsync();

            await _presenter.OnNextPageAsync();
            _client.Invocations.Clear();

            await _presenter.OnSearchDebounceElapsedAsync();

            _client.Verify(c => c.GetDocumentsPaged(
                It.Is<DocumentQueryRequest>(r => r.PageNumber == 0)), Times.Once);
        }

        [TestMethod]
        public void OnDismissWarning_HidesWarning()
        {
            _presenter.OnDismissWarning();

            _view.Verify(v => v.HideWarning(), Times.Once);
        }

        [TestMethod]
        public async Task OnUploadCompletedAsync_ReloadData()
        {
            await InitializePresenterAsync();

            await _presenter.OnUploadCompletedAsync();

            _client.Verify(c => c.GetDocumentsPaged(It.IsAny<DocumentQueryRequest>()), Times.Once);
        }

        [TestMethod]
        public async Task OnClosing_SavesCurrentPreferencesToStore()
        {
            await InitializePresenterAsync();

            _presenter.OnClosing();

            _preferencesStore.Verify(s => s.Save(_preferences), Times.Once);
        }

        #endregion

        #region Properties

        [TestMethod]
        public void InstitutionId_ReturnsValuePassedToConstructor()
        {
            Assert.AreEqual(1, _presenter.InstitutionId);
        }

        #endregion
    }
}