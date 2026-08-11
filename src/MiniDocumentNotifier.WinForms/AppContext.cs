using System;
using System.Configuration;
using System.Windows.Forms;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Infrastructure.Concurrency;
using MiniDocumentNotifier.Infrastructure.Preferences;
using MiniDocumentNotifier.WinForms.Forms;
using MiniDocumentNotifier.WinForms.Wizard;

namespace MiniDocumentNotifier.WinForms
{
    public class AppContext : ApplicationContext
    {
        private bool _isBackgroundAppRunning;
        private bool _loginSucceeded;
        private readonly LoginWizardState _loginWizardState = new LoginWizardState();
        private readonly IUserPreferencesStore _userPreferencesStore;
        private readonly UserPreferences _userPreferences;

        public AppContext()
        {
            var path = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["UserPreferencesPath"]);
            _userPreferencesStore = new JsonUserPreferencesStore(path);
            _userPreferences = _userPreferencesStore.Load();
            _loginWizardState.Username = _userPreferences.LastUsername;

            var splashScreen = new SplashScreenForm();
            splashScreen.FormClosed += SplashScreenForm_FormClosed;
            splashScreen.Show();
        }

        private void SplashScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isBackgroundAppRunning = CheckBackgroundAppRunning();

            var loginWizardForm = new LoginWizardForm(_loginWizardState);
            loginWizardForm.LoginSucceeded += () => _loginSucceeded = true;
            loginWizardForm.FormClosed += LoginWizardForm_FormClosed;
            loginWizardForm.Show();
        }

        private void LoginWizardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_loginSucceeded)
            {
                ExitThread();
                return;
            }

            _userPreferences.LastUsername = _loginWizardState.Username;
            _userPreferencesStore.Save(_userPreferences);

            var mainForm = new MainForm(_isBackgroundAppRunning, _loginWizardState.InstitutionId, _userPreferencesStore);
            mainForm.FormClosed += (s, args) => { ExitThread(); };
            mainForm.Show();
        }

        private static bool CheckBackgroundAppRunning()
        {
            using (var signal = new SemaphoreBackgroundAppSignal(Constants.BackgroundAppSemaphoreName))
            {
                return signal.IsActive();
            }
        }
    }
}