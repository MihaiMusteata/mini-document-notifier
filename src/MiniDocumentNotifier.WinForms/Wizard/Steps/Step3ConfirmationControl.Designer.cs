using System.ComponentModel;

namespace MiniDocumentNotifier.WinForms.Wizard.Steps
{
    partial class Step3ConfirmationControl
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
            this.lblInstitution = new System.Windows.Forms.Label();
            this.txtInstitution = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.grpConfirmation = new System.Windows.Forms.GroupBox();
            this.grpConfirmation.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInstitution
            // 
            this.lblInstitution.Location = new System.Drawing.Point(43, 27);
            this.lblInstitution.Name = "lblInstitution";
            this.lblInstitution.Size = new System.Drawing.Size(100, 15);
            this.lblInstitution.TabIndex = 0;
            this.lblInstitution.Text = "Institution";
            // 
            // txtInstitution
            // 
            this.txtInstitution.Location = new System.Drawing.Point(43, 45);
            this.txtInstitution.Margin = new System.Windows.Forms.Padding(0);
            this.txtInstitution.Name = "txtInstitution";
            this.txtInstitution.ReadOnly = true;
            this.txtInstitution.Size = new System.Drawing.Size(301, 20);
            this.txtInstitution.TabIndex = 1;
            this.txtInstitution.Text = "TEST";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(43, 90);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.ReadOnly = true;
            this.txtUsername.Size = new System.Drawing.Size(301, 20);
            this.txtUsername.TabIndex = 3;
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(43, 72);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(100, 15);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(269, 112);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // grpConfirmation
            // 
            this.grpConfirmation.Controls.Add(this.btnLogin);
            this.grpConfirmation.Controls.Add(this.txtUsername);
            this.grpConfirmation.Controls.Add(this.lblUsername);
            this.grpConfirmation.Controls.Add(this.txtInstitution);
            this.grpConfirmation.Controls.Add(this.lblInstitution);
            this.grpConfirmation.Location = new System.Drawing.Point(210, 130);
            this.grpConfirmation.Name = "grpConfirmation";
            this.grpConfirmation.Size = new System.Drawing.Size(365, 145);
            this.grpConfirmation.TabIndex = 5;
            this.grpConfirmation.TabStop = false;
            this.grpConfirmation.Text = "Confirmation";
            // 
            // Step3ConfirmationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpConfirmation);
            this.Name = "Step3ConfirmationControl";
            this.Size = new System.Drawing.Size(785, 405);
            this.grpConfirmation.ResumeLayout(false);
            this.grpConfirmation.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox grpConfirmation;

        private System.Windows.Forms.TextBox txtInstitution;

        private System.Windows.Forms.Label lblUsername;

        private System.Windows.Forms.TextBox txtUsername;

        private System.Windows.Forms.Button btnLogin;

        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.TextBox textBox1;

        private System.Windows.Forms.Label lblInstitution;

        #endregion
    }
}