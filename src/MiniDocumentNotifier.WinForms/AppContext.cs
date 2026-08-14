using System.Threading.Tasks;
using System.Windows.Forms;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Infrastructure.Concurrency;
using MiniDocumentNotifier.WinForms.Forms;
using MiniDocumentNotifier.WinForms.Wizard;

namespace MiniDocumentNotifier.WinForms
{
    public class AppContext : ApplicationContext
    {
        private readonly IUserPreferencesStore _userPreferencesStore;
        private readonly IBackgroundAppSignal _backgroundAppSignal;
        private readonly IViewConfigurationStore _viewConfigurationStore;
        private readonly LoginWizardState _loginWizardState = new LoginWizardState();

        private bool _isBackgroundAppRunning;
        private bool _loginSucceeded;
        private UserPreferences _userPreferences;

        public AppContext(
            IUserPreferencesStore userPreferencesStore,
            IBackgroundAppSignal backgroundAppSignal,
            IViewConfigurationStore viewConfigurationStore)
        {
            _userPreferencesStore = userPreferencesStore;
            _backgroundAppSignal = backgroundAppSignal;
            _viewConfigurationStore = viewConfigurationStore;

            var splashScreen = new SplashScreenForm();
            splashScreen.InitializationSteps += () => RunStartupSequence(splashScreen);
            splashScreen.FormClosed += SplashScreenForm_FormClosed;
            splashScreen.Show();
        }

        private async Task RunStartupSequence(SplashScreenForm splashScreen)
        {
            splashScreen.SetStatus("The preferences are loading...");
            _userPreferences = await Task.Run(() => _userPreferencesStore.Load());
            _loginWizardState.Username = _userPreferences.LastUsername;

            splashScreen.SetStatus("Checking Background App...");
            _isBackgroundAppRunning = await Task.Run(() => CheckBackgroundAppRunning(_backgroundAppSignal));
        }

        private void SplashScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
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

            var mainForm = new MainForm(_userPreferencesStore, _viewConfigurationStore, _isBackgroundAppRunning, _loginWizardState.InstitutionId);

            mainForm.FormClosed += (s, args) => { ExitThread(); };
            mainForm.Show();
        }

        private static bool CheckBackgroundAppRunning(IBackgroundAppSignal signal)
        {
            using (signal)
            {
                return signal.IsActive();
            }
        }
    }
}