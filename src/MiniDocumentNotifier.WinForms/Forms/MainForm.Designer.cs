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
            this.documentsDataGrid = new System.Windows.Forms.DataGridView();
            this.warningPanel = new System.Windows.Forms.Panel();
            this.lblWarning = new System.Windows.Forms.Label();
            this.btnDismissWarning = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGrid)).BeginInit();
            this.warningPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // documentsDataGrid
            //
            this.documentsDataGrid.AllowUserToAddRows = false;
            this.documentsDataGrid.AllowUserToDeleteRows = false;
            this.documentsDataGrid.AllowUserToResizeColumns = true;
            this.documentsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.documentsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.documentsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentsDataGrid.Location = new System.Drawing.Point(0, 0);
            this.documentsDataGrid.MultiSelect = false;
            this.documentsDataGrid.Name = "documentsDataGrid";
            this.documentsDataGrid.ReadOnly = true;
            this.documentsDataGrid.RowHeadersVisible = false;
            this.documentsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.documentsDataGrid.Size = new System.Drawing.Size(800, 420);
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
            this.lblWarning.Text = "";
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
            // MainForm
            //
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.documentsDataGrid);
            this.Controls.Add(this.warningPanel);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGrid)).EndInit();
            this.warningPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView documentsDataGrid;
        private System.Windows.Forms.Panel warningPanel;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Button btnDismissWarning;

        #endregion
    }
}