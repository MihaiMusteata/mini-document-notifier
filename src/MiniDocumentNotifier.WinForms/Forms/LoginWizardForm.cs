using System;
using System.Windows.Forms;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.WinForms.Wizard;
using MiniDocumentNotifier.WinForms.Wizard.Steps;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class LoginWizardForm : Form
    {
        private const int MinStep = 0;
        private const int MaxStep = 2;

        private int _currentStep = MinStep;

        public event Action LoginSucceeded;

        public LoginWizardForm(LoginWizardState loginWizardState, ILogger logger)
        {
            InitializeComponent();

            var step3 = new Step3ConfirmationControl(loginWizardState, logger);
            step3.LoginSucceeded += () =>
            {
                LoginSucceeded?.Invoke();
                Close();
            };

            contentPanel.Controls.Add(new Step1InstitutionControl(loginWizardState));
            contentPanel.Controls.Add(new Step2CredentialsControl(loginWizardState));
            contentPanel.Controls.Add(step3);

            ShowStep(_currentStep);
        }

        private void ShowStep(int step)
        {
            for (var i = 0; i < contentPanel.Controls.Count; i++)
            {
                contentPanel.Controls[i].Visible = (i == step);
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

            if (contentPanel.Controls[_currentStep] is ILoginWizardStep currentStep)
            {
                currentStep.SaveData();
            }

            _currentStep++;
            ShowStep(_currentStep);
        }
    }
}