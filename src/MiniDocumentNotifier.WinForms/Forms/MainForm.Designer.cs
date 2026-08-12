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
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGrid)).BeginInit();
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
            this.documentsDataGrid.Size = new System.Drawing.Size(800, 450);
            this.documentsDataGrid.TabIndex = 0;
            this.documentsDataGrid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.documentsDataGrid_ColumnHeaderMouseClick);
            this.documentsDataGrid.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.documentsDataGrid_ColumnWidthChanged);
            // 
            // MainForm
            // 
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.documentsDataGrid);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGrid)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView documentsDataGrid;

        #endregion
    }
}