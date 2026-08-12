using System.ComponentModel;

namespace MiniDocumentNotifier.WinForms.Forms
{
    partial class LoginWizardForm
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
            this.contentPanel = new System.Windows.Forms.Panel();
            this.btnStepNext = new System.Windows.Forms.Button();
            this.btnStepBack = new System.Windows.Forms.Button();
            this.navPanel = new System.Windows.Forms.Panel();
            this.navPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(784, 461);
            this.contentPanel.TabIndex = 0;
            // 
            // btnStepNext
            // 
            this.btnStepNext.Location = new System.Drawing.Point(697, 14);
            this.btnStepNext.Name = "btnStepNext";
            this.btnStepNext.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnStepNext.Size = new System.Drawing.Size(75, 23);
            this.btnStepNext.TabIndex = 1;
            this.btnStepNext.Text = "Next";
            this.btnStepNext.UseVisualStyleBackColor = true;
            this.btnStepNext.Click += new System.EventHandler(this.btnStepNext_Click);
            // 
            // btnStepBack
            // 
            this.btnStepBack.Location = new System.Drawing.Point(12, 14);
            this.btnStepBack.Name = "btnStepBack";
            this.btnStepBack.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnStepBack.Size = new System.Drawing.Size(75, 23);
            this.btnStepBack.TabIndex = 0;
            this.btnStepBack.Text = "Back";
            this.btnStepBack.UseVisualStyleBackColor = true;
            this.btnStepBack.Click += new System.EventHandler(this.btnStepBack_Click);
            // 
            // navPanel
            // 
            this.navPanel.Controls.Add(this.btnStepNext);
            this.navPanel.Controls.Add(this.btnStepBack);
            this.navPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.navPanel.Location = new System.Drawing.Point(0, 405);
            this.navPanel.Name = "navPanel";
            this.navPanel.Size = new System.Drawing.Size(784, 56);
            this.navPanel.TabIndex = 2;
            // 
            // LoginWizardForm
            // 
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.navPanel);
            this.Controls.Add(this.contentPanel);
            this.Name = "LoginWizardForm";
            this.Text = "LoginWizardForm";
            this.navPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel navPanel;

        private System.Windows.Forms.Button btnStepBack;

        private System.Windows.Forms.Button btnStepNext;

        private System.Windows.Forms.Panel contentPanel;

        #endregion
    }
}