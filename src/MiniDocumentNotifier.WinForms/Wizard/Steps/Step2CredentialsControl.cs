using System;
using System.Windows.Forms;

namespace MiniDocumentNotifier.WinForms.Wizard.Steps
{
    public partial class Step2CredentialsControl : UserControl, ILoginWizardStep
    {
        private readonly LoginWizardState _loginWizardState;
        public Step2CredentialsControl(LoginWizardState loginWizardState)
        {
            InitializeComponent();
            _loginWizardState = loginWizardState;
        }

        public void SaveData()
        {
            _loginWizardState.Username = txtUsername.Text;
            _loginWizardState.Password = txtPassword.Text;
            
        }
    }
}