using System.Drawing;
using System.Windows.Forms;
using MiniDocumentNotifier.WpfControls.ViewModels;
using MiniDocumentNotifier.WpfControls.Views;
using System.Windows.Forms.Integration;

namespace MiniDocumentNotifier.WinForms.Wizard.Steps
{
    public partial class Step2CredentialsControl : UserControl, ILoginWizardStep
    {
        private readonly LoginWizardState _loginWizardState;
        private readonly Step2CredentialsViewModel _viewModel;

        public Step2CredentialsControl(LoginWizardState loginWizardState)
        {
            InitializeComponent();
            _loginWizardState = loginWizardState;

            _viewModel = new Step2CredentialsViewModel
            {
                Username = _loginWizardState.Username
            };

            var view = new Step2CredentialsView
            {
                DataContext = _viewModel
            };

            var elementHost = new ElementHost
            {
                Location = new Point(210, 130),
                Size = new Size(365, 145),
                Child = view
            };

            Controls.Add(elementHost);
        }

        public void SaveData()
        {
            _loginWizardState.Username = _viewModel.Username;
            _loginWizardState.Password = _viewModel.Password;
        }
    }
}