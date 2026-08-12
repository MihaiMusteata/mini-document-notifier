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

            if (!string.IsNullOrEmpty(_loginWizardState.Username))
                txtUsername.Text = _loginWizardState.Username;
        }

        public void SaveData()
        {
            _loginWizardState.Username = txtUsername.Text;
            _loginWizardState.Password = txtPassword.Text;
            
        }
    }
}