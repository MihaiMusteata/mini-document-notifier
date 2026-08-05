using System;
using System.Windows.Forms;

namespace MiniDocumentNotifier.WinForms.Wizard.Steps
{
    public partial class Step2CredentialsControl : UserControl
    {
        private LoginWizardState _loginWizardState;
        public Step2CredentialsControl(LoginWizardState loginWizardState)
        {
            _loginWizardState = loginWizardState;
            InitializeComponent();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            _loginWizardState.Username = txtUsername.Text;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            _loginWizardState.Password = txtPassword.Text;
        }
    }
}