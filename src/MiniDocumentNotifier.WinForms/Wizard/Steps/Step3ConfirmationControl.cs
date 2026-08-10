using System;
using System.Windows.Forms;

namespace MiniDocumentNotifier.WinForms.Wizard.Steps
{
    public partial class Step3ConfirmationControl : UserControl, ILoginWizardStep
    {
        private readonly LoginWizardState _loginWizardState;

        public Step3ConfirmationControl(LoginWizardState loginWizardState)
        {
            InitializeComponent();
            _loginWizardState = loginWizardState;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!Visible) return;

            txtInstitution.Text = _loginWizardState.InstitutionName;
            txtUsername.Text = _loginWizardState.Username;
        }

        public void SaveData()
        {
        }
    }
}