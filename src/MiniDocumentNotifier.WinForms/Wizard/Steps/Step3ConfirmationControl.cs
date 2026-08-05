using System;
using System.Windows.Forms;

namespace MiniDocumentNotifier.WinForms.Wizard.Steps
{
    public partial class Step3ConfirmationControl : UserControl
    {
        private LoginWizardState _loginWizardState;

        public Step3ConfirmationControl(LoginWizardState loginWizardState)
        {
            _loginWizardState = loginWizardState;
            InitializeComponent();

            txtInstitution.Text = _loginWizardState.InstitutionName;
            txtUsername.Text = _loginWizardState.Username;
        }
    }
}