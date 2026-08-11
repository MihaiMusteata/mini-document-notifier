using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniDocumentNotifier.Contracts.InstitutionContracts;
using MiniDocumentNotifier.WinForms.Services;

namespace MiniDocumentNotifier.WinForms.Wizard.Steps
{
    public partial class Step1InstitutionControl : UserControl, ILoginWizardStep
    {
        private readonly LoginWizardState _loginWizardState;
        private bool _loaded;

        public Step1InstitutionControl(LoginWizardState loginWizardState)
        {
            InitializeComponent();
            _loginWizardState = loginWizardState;

            Load += async (s, e) => await LoadInstitutionsAsync();
        }

        public void SaveData()
        {
            var institution = (InstitutionDto)cmbSelectInstitution.SelectedItem;

            if (institution == null) return;

            _loginWizardState.InstitutionName = institution.Name;
            _loginWizardState.InstitutionId = institution.Id;
        }

        private async Task LoadInstitutionsAsync()
        {
            if (_loaded) return;

            cmbSelectInstitution.Enabled = false;

            try
            {
                var institutions = await Task.Run(() =>
                {
                    using (var client = new DocumentNotifierServiceClient())
                    {
                        return client.GetInstitutions();
                    }
                });

                cmbSelectInstitution.DataSource = institutions;
                cmbSelectInstitution.ValueMember = "Id";
                cmbSelectInstitution.DisplayMember = "Name";
                cmbSelectInstitution.Enabled = true;
                _loaded = true;
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(this, "Service is not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (CommunicationException)
            {
                MessageBox.Show(this, "Communication error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cmbSelectInstitution.Enabled = true;
            }
        }
    }
}