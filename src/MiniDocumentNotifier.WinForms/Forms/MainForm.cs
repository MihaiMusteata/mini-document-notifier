using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MiniDocumentNotifier.Application.Document;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Infrastructure.ServiceClient;
using MiniDocumentNotifier.WinForms.Models;
using MiniDocumentNotifier.WinForms.Presenters;
using MiniDocumentNotifier.WinForms.Services;
using MiniDocumentNotifier.WinForms.UnityBootstrapper;
using MiniDocumentNotifier.WinForms.Views;
using Unity;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class MainForm : Form, IMainView
    {
        private readonly MainPresenter _presenter;
        private readonly IDocumentNotifierServiceClient _serviceClient;
        private bool _suppressWidthCapture;

        public MainForm(
            IUserPreferencesStore preferencesStore,
            IViewConfigurationStore viewConfigurationStore,
            bool isBackgroundAppRunning,
            int institutionId) :
            this(
                preferencesStore,
                viewConfigurationStore,
                new DocumentNotifierServiceClient(),
                isBackgroundAppRunning,
                institutionId)
        {
        }

        public MainForm(IUserPreferencesStore preferencesStore, IViewConfigurationStore viewConfigurationStore,
            IDocumentNotifierServiceClient serviceClient, bool isBackgroundAppRunning, int institutionId)
        {
            _serviceClient = serviceClient;
            InitializeComponent();

            _presenter = new MainPresenter(this, preferencesStore, viewConfigurationStore,
                serviceClient, isBackgroundAppRunning, institutionId);

            searchDebounceTimer.Interval = 500;

            Load += async (s, e) => await _presenter.InitializeAsync();
            FormClosing += (s, e) =>
            {
                _presenter.OnClosing();
                serviceClient.Dispose();
                searchDebounceTimer.Dispose();
            };
        }


        #region IMainView Implementation

        public string SearchText => txtSearch.Text;
        public string SelectedTypeFilterLabel => cmbTypeFilter.SelectedItem?.ToString();
        public string SelectedStatusFilterLabel => cmbStatusFilter.SelectedItem?.ToString();

        public List<DocumentRow> Rows
        {
            set
            {
                documentsDataGrid.DataSource = null;
                documentsDataGrid.DataSource = value;
            }
        }

        public void SetTypeFilterOptions(List<string> options) => cmbTypeFilter.DataSource = options.ToList();

        public void SetStatusFilterOptions(List<string> options) => cmbStatusFilter.DataSource = options.ToList();

        public void SetPagingInfo(string pageText, bool prevEnabled, bool nextEnabled)
        {
            lblPageInfo.Text = pageText;
            btnPrevPage.Enabled = prevEnabled;
            btnNextPage.Enabled = nextEnabled;
        }

        public void DisplayDocuments(List<DocumentRow> rows, IReadOnlyDictionary<string, float> columnWidths)
        {
            _suppressWidthCapture = true;

            documentsDataGrid.DataSource = null;
            documentsDataGrid.DataSource = rows;

            foreach (DataGridViewColumn column in documentsDataGrid.Columns)
            {
                if (columnWidths.TryGetValue(column.Name, out var weight))
                    column.FillWeight = weight;
            }

            _suppressWidthCapture = false;
        }

        public void ApplyColumnWidth(string columnName, float weight)
        {
            if (!documentsDataGrid.Columns.Contains(columnName)) return;

            _suppressWidthCapture = true;
            documentsDataGrid.Columns[columnName].FillWeight = weight;
            _suppressWidthCapture = false;
        }

        public void ShowWarning(string message)
        {
            lblWarning.Text = message;
            warningPanel.Visible = true;
        }

        public void HideWarning() => warningPanel.Visible = false;

        public void ShowTimeoutError() => MessageBox.Show(this, @"Service timeout.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public void SetSortIndicator(string columnName, bool descending)
        {
            if (!documentsDataGrid.Columns.Contains(columnName)) return;

            documentsDataGrid.Columns[columnName].HeaderCell.SortGlyphDirection =
                descending ? SortOrder.Descending : SortOrder.Ascending;
        }

        #endregion

        #region Event Handlers (delegated to presenter)

        private void btnDismissWarning_Click(object sender, System.EventArgs e) => _presenter.OnDismissWarning();

        private async void documentsDataGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var columnName = documentsDataGrid.Columns[e.ColumnIndex].Name;
            await _presenter.OnColumnHeaderClickedAsync(columnName);
        }

        private void documentsDataGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (_suppressWidthCapture) return;
            _presenter.OnColumnWidthChanged(e.Column.Name, e.Column.FillWeight);
        }

        private async void btnPrevPage_Click(object sender, System.EventArgs e) =>
            await _presenter.OnPrevPageAsync();

        private async void btnNextPage_Click(object sender, System.EventArgs e) =>
            await _presenter.OnNextPageAsync();

        private async void cmbTypeFilter_SelectedIndexChanged(object sender, System.EventArgs e) =>
            await _presenter.OnTypeFilterChangedAsync(SelectedTypeFilterLabel);

        private async void cmbStatusFilter_SelectedIndexChanged(object sender, System.EventArgs e) =>
            await _presenter.OnStatusFilterChangedAsync(SelectedStatusFilterLabel);

        private void txtSearch_TextChanged(object sender, System.EventArgs e)
        {
            searchDebounceTimer.Stop();
            searchDebounceTimer.Start();
        }

        private async void searchDebounceTimer_Tick(object sender, System.EventArgs e)
        {
            searchDebounceTimer.Stop();
            await _presenter.OnSearchDebounceElapsedAsync();
        }

        private void uploadToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var fileStorage = Bootstrapper.Container.Resolve<IFileStorage>();
            
            using (var uploadForm = new DocumentUploadForm(_presenter.InstitutionId, _serviceClient, fileStorage))
            {
                if (uploadForm.ShowDialog(this) == DialogResult.OK)
                {
                    _ = _presenter.OnUploadCompletedAsync();
                }
            }
        }

        #endregion
    }
}