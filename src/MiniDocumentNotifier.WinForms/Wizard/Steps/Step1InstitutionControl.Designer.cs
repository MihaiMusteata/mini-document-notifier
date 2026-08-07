using System.ComponentModel;

namespace MiniDocumentNotifier.WinForms.Wizard.Steps
{
    partial class Step1InstitutionControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbSelectInstitution = new System.Windows.Forms.ComboBox();
            this.lblSelectInstitution = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbSelectInstitution
            // 
            this.cmbSelectInstitution.FormattingEnabled = true;
            this.cmbSelectInstitution.Items.AddRange(new object[] { "1", "2", "an" });
            this.cmbSelectInstitution.Location = new System.Drawing.Point(123, 130);
            this.cmbSelectInstitution.Name = "cmbSelectInstitution";
            this.cmbSelectInstitution.Size = new System.Drawing.Size(301, 21);
            this.cmbSelectInstitution.TabIndex = 0;
            // 
            // lblSelectInstitution
            // 
            this.lblSelectInstitution.Location = new System.Drawing.Point(123, 112);
            this.lblSelectInstitution.Name = "lblSelectInstitution";
            this.lblSelectInstitution.Size = new System.Drawing.Size(100, 15);
            this.lblSelectInstitution.TabIndex = 1;
            this.lblSelectInstitution.Text = "Select institution";
            // 
            // Step1InstitutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSelectInstitution);
            this.Controls.Add(this.cmbSelectInstitution);
            this.Name = "Step1InstitutionControl";
            this.Size = new System.Drawing.Size(785, 405);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblSelectInstitution;

        private System.Windows.Forms.ComboBox cmbSelectInstitution;

        #endregion
    }
}