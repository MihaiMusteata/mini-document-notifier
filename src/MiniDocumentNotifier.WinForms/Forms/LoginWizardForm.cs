using System;
using System.Windows.Forms;
using MiniDocumentNotifier.WinForms.Wizard;
using MiniDocumentNotifier.WinForms.Wizard.Steps;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class LoginWizardForm : Form
    {
        private const int MinStep = 0;
        private const int MaxStep = 2;

        private int _currentStep = 0;

        private readonly LoginWizardState _loginWizardState;

        public LoginWizardForm()
        {
            InitializeComponent();

            _loginWizardState = new LoginWizardState();

            ShowStep(_currentStep);
        }

        private void ShowStep(int step)
        {
            contentPanel.Controls.Clear();

            switch (step)
            {
                case 0:
                    contentPanel.Controls.Add(new Step1InstitutionControl(_loginWizardState));
                    break;
                case 1:
                    contentPanel.Controls.Add(new Step2CredentialsControl(_loginWizardState));
                    break;
                case 2:
                    contentPanel.Controls.Add(new Step3ConfirmationControl(_loginWizardState));
                    break;
            }

            btnStepBack.Enabled = step > MinStep;
            btnStepNext.Enabled = step < MaxStep;
        }


        private void btnStepBack_Click(object sender, EventArgs e)
        {
            if (_currentStep <= MinStep) return;

            _currentStep--;
            ShowStep(_currentStep);
        }

        private void btnStepNext_Click(object sender, EventArgs e)
        {
            if (_currentStep >= MaxStep) return;

            _currentStep++;
            ShowStep(_currentStep);
        }
    }
}