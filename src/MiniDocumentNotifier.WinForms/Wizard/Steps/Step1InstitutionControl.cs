using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MiniDocumentNotifier.WinForms.Wizard.Steps
{
    public class InstitutionOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public partial class Step1InstitutionControl : UserControl
    {
   
        private LoginWizardState _loginWizardState;
        // TODO: Change mock to real data
        private readonly List<InstitutionOption> MockInstitutions;
        public Step1InstitutionControl(LoginWizardState loginWizardState)
        {
            InitializeComponent();
            MockInstitutions = new List<InstitutionOption>()
            {
                new InstitutionOption { Id = 1, Name = "Moldova Agroindbank" },
                new InstitutionOption { Id = 2, Name = "Moldindconbank" },
                new InstitutionOption { Id = 3, Name = "Victoriabank" }
            };

            _loginWizardState = loginWizardState;

            cmbSelectInstitution.DataSource = MockInstitutions;
            cmbSelectInstitution.ValueMember = "Id";
            cmbSelectInstitution.DisplayMember =  "Name";
            
        }

        private void cmbSelectInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            var institution = (InstitutionOption)cmbSelectInstitution.SelectedItem;
            
            if (institution == null) return;

            _loginWizardState.InstitutionName = institution.Name;
            _loginWizardState.InstitutionId = institution.Id;
        }
    }
}