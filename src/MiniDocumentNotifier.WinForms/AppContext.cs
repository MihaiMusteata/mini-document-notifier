using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniDocumentNotifier.Domain.Abstractions;
using MiniDocumentNotifier.Domain.Models;
using MiniDocumentNotifier.Infrastructure.Concurrency;
using MiniDocumentNotifier.WinForms.Forms;
using MiniDocumentNotifier.WinForms.UnityBootstrapper;
using MiniDocumentNotifier.WinForms.Wizard;
using Unity;
using Unity.Resolution;

namespace MiniDocumentNotifier.WinForms
{
    public class AppContext : ApplicationContext
    {
        private bool _isBackgroundAppRunning;
        private bool _loginSucceeded;
        private readonly LoginWizardState _loginWizardState = new LoginWizardState();
        private readonly IUserPreferencesStore _userPreferencesStore;
        private UserPreferences _userPreferences;

        public AppContext()
        {
            _userPreferencesStore = Bootstrapper.Container.Resolve<IUserPreferencesStore>();

            var splashScreen = Bootstrapper.Container.Resolve<SplashScreenForm>();
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
            _isBackgroundAppRunning = await Task.Run(CheckBackgroundAppRunning);
        }

        private void SplashScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var loginWizardForm = Bootstrapper.Container.Resolve<LoginWizardForm>(
                new ParameterOverride("loginWizardState", _loginWizardState));

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

            var mainForm = Bootstrapper.Container.Resolve<MainForm>(
                new ParameterOverride("isBackgroundAppRunning", _isBackgroundAppRunning),
                new ParameterOverride("institutionId", _loginWizardState.InstitutionId));

            mainForm.FormClosed += (s, args) => { ExitThread(); };
            mainForm.Show();
        }

        private static bool CheckBackgroundAppRunning()
        {
            using (var signal = new SemaphoreBackgroundAppSignal(Constants.BackgroundAppSemaphoreName))
            {
                // just for checking if UI doesn't freeze during this call
                Thread.Sleep(2000);
                return signal.IsActive();
            }
        }
    }
}