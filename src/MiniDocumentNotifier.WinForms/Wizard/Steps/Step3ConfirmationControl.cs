using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniDocumentNotifier.Contracts.AuthContracts;
using MiniDocumentNotifier.WinForms.Services;

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

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;

            try
            {
                var request = new LoginRequest
                {
                    InstitutionId = _loginWizardState.InstitutionId,
                    Username = _loginWizardState.Username,
                    Password = _loginWizardState.Password
                };

                var result = await Task.Run(() =>
                {
                    using (var client = new DocumentNotifierServiceClient())
                    {
                        return client.Login(request);
                    }
                });

                MessageBox.Show(this, "Logged in succssefully", "Login", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                var wizard = FindForm();
                wizard?.Close();
            }
            catch (FaultException<AuthFault> fault)
            {
                MessageBox.Show(this, fault.Detail.Message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(this, "Communication error with service", "Eroare", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }
    }
}