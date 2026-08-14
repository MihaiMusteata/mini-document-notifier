using System.ComponentModel;

namespace MiniDocumentNotifier.WinForms.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.documentsDataGrid = new System.Windows.Forms.DataGridView();
            this.warningPanel = new System.Windows.Forms.Panel();
            this.lblWarning = new System.Windows.Forms.Label();
            this.btnDismissWarning = new System.Windows.Forms.Button();
            this.filterPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblTypeFilter = new System.Windows.Forms.Label();
            this.cmbTypeFilter = new System.Windows.Forms.ComboBox();
            this.lblStatusFilter = new System.Windows.Forms.Label();
            this.cmbStatusFilter = new System.Windows.Forms.ComboBox();
            this.paginationPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.searchDebounceTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGrid)).BeginInit();
            this.warningPanel.SuspendLayout();
            this.filterPanel.SuspendLayout();
            this.paginationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // documentsDataGrid
            // 
            this.documentsDataGrid.AllowUserToAddRows = false;
            this.documentsDataGrid.AllowUserToDeleteRows = false;
            this.documentsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.documentsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.documentsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentsDataGrid.Location = new System.Drawing.Point(0, 71);
            this.documentsDataGrid.MultiSelect = false;
            this.documentsDataGrid.Name = "documentsDataGrid";
            this.documentsDataGrid.ReadOnly = true;
            this.documentsDataGrid.RowHeadersVisible = false;
            this.documentsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.documentsDataGrid.Size = new System.Drawing.Size(800, 337);
            this.documentsDataGrid.TabIndex = 0;
            this.documentsDataGrid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.documentsDataGrid_ColumnHeaderMouseClick);
            this.documentsDataGrid.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.documentsDataGrid_ColumnWidthChanged);
            // 
            // warningPanel
            // 
            this.warningPanel.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.warningPanel.Controls.Add(this.lblWarning);
            this.warningPanel.Controls.Add(this.btnDismissWarning);
            this.warningPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.warningPanel.Location = new System.Drawing.Point(0, 0);
            this.warningPanel.Name = "warningPanel";
            this.warningPanel.Size = new System.Drawing.Size(800, 30);
            this.warningPanel.TabIndex = 1;
            this.warningPanel.Visible = false;
            // 
            // lblWarning
            // 
            this.lblWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWarning.Location = new System.Drawing.Point(0, 0);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblWarning.Size = new System.Drawing.Size(776, 30);
            this.lblWarning.TabIndex = 0;
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDismissWarning
            // 
            this.btnDismissWarning.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDismissWarning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDismissWarning.Location = new System.Drawing.Point(776, 0);
            this.btnDismissWarning.Name = "btnDismissWarning";
            this.btnDismissWarning.Size = new System.Drawing.Size(24, 30);
            this.btnDismissWarning.TabIndex = 1;
            this.btnDismissWarning.Text = "×";
            this.btnDismissWarning.UseVisualStyleBackColor = true;
            this.btnDismissWarning.Click += new System.EventHandler(this.btnDismissWarning_Click);
            // 
            // filterPanel
            // 
            this.filterPanel.AutoSize = true;
            this.filterPanel.Controls.Add(this.lblSearch);
            this.filterPanel.Controls.Add(this.txtSearch);
            this.filterPanel.Controls.Add(this.lblTypeFilter);
            this.filterPanel.Controls.Add(this.cmbTypeFilter);
            this.filterPanel.Controls.Add(this.lblStatusFilter);
            this.filterPanel.Controls.Add(this.cmbStatusFilter);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Location = new System.Drawing.Point(0, 30);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.filterPanel.Size = new System.Drawing.Size(800, 41);
            this.filterPanel.TabIndex = 2;
            this.filterPanel.WrapContents = false;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(8, 14);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(44, 13);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(56, 10);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 4, 16, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(180, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblTypeFilter
            // 
            this.lblTypeFilter.AutoSize = true;
            this.lblTypeFilter.Location = new System.Drawing.Point(252, 14);
            this.lblTypeFilter.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblTypeFilter.Name = "lblTypeFilter";
            this.lblTypeFilter.Size = new System.Drawing.Size(34, 13);
            this.lblTypeFilter.TabIndex = 2;
            this.lblTypeFilter.Text = "Type:";
            // 
            // cmbTypeFilter
            // 
            this.cmbTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypeFilter.Location = new System.Drawing.Point(290, 10);
            this.cmbTypeFilter.Margin = new System.Windows.Forms.Padding(0, 4, 16, 4);
            this.cmbTypeFilter.Name = "cmbTypeFilter";
            this.cmbTypeFilter.Size = new System.Drawing.Size(130, 21);
            this.cmbTypeFilter.TabIndex = 3;
            this.cmbTypeFilter.SelectedIndexChanged += new System.EventHandler(this.cmbTypeFilter_SelectedIndexChanged);
            // 
            // lblStatusFilter
            // 
            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Location = new System.Drawing.Point(436, 14);
            this.lblStatusFilter.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblStatusFilter.Name = "lblStatusFilter";
            this.lblStatusFilter.Size = new System.Drawing.Size(40, 13);
            this.lblStatusFilter.TabIndex = 4;
            this.lblStatusFilter.Text = "Status:";
            // 
            // cmbStatusFilter
            // 
            this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusFilter.Location = new System.Drawing.Point(480, 10);
            this.cmbStatusFilter.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.cmbStatusFilter.Name = "cmbStatusFilter";
            this.cmbStatusFilter.Size = new System.Drawing.Size(130, 21);
            this.cmbStatusFilter.TabIndex = 5;
            this.cmbStatusFilter.SelectedIndexChanged += new System.EventHandler(this.cmbStatusFilter_SelectedIndexChanged);
            // 
            // paginationPanel
            // 
            this.paginationPanel.AutoSize = true;
            this.paginationPanel.Controls.Add(this.btnPrevPage);
            this.paginationPanel.Controls.Add(this.lblPageInfo);
            this.paginationPanel.Controls.Add(this.btnNextPage);
            this.paginationPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paginationPanel.Location = new System.Drawing.Point(0, 408);
            this.paginationPanel.Name = "paginationPanel";
            this.paginationPanel.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.paginationPanel.Size = new System.Drawing.Size(800, 42);
            this.paginationPanel.TabIndex = 3;
            this.paginationPanel.WrapContents = false;
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Location = new System.Drawing.Point(8, 9);
            this.btnPrevPage.Margin = new System.Windows.Forms.Padding(0, 3, 8, 3);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(75, 24);
            this.btnPrevPage.TabIndex = 0;
            this.btnPrevPage.Text = "< Prev";
            this.btnPrevPage.UseVisualStyleBackColor = true;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(91, 14);
            this.lblPageInfo.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(62, 13);
            this.lblPageInfo.TabIndex = 1;
            this.lblPageInfo.Text = "Page 1 of 1";
            // 
            // btnNextPage
            // 
            this.btnNextPage.Location = new System.Drawing.Point(161, 9);
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(75, 24);
            this.btnNextPage.TabIndex = 2;
            this.btnNextPage.Text = "Next >";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // searchDebounceTimer
            // 
            this.searchDebounceTimer.Tick += new System.EventHandler(this.searchDebounceTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.documentsDataGrid);
            this.Controls.Add(this.paginationPanel);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(this.warningPanel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGrid)).EndInit();
            this.warningPanel.ResumeLayout(false);
            this.filterPanel.ResumeLayout(false);
            this.filterPanel.PerformLayout();
            this.paginationPanel.ResumeLayout(false);
            this.paginationPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Timer searchDebounceTimer;

        private System.Windows.Forms.Timer timer1;

        private System.Windows.Forms.DataGridView documentsDataGrid;
        private System.Windows.Forms.Panel warningPanel;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Button btnDismissWarning;
        private System.Windows.Forms.FlowLayoutPanel filterPanel;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblTypeFilter;
        private System.Windows.Forms.ComboBox cmbTypeFilter;
        private System.Windows.Forms.Label lblStatusFilter;
        private System.Windows.Forms.ComboBox cmbStatusFilter;
        private System.Windows.Forms.FlowLayoutPanel paginationPanel;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnNextPage;

        #endregion
    }
}
