using System.Windows.Forms;
using MiniDocumentNotifier.Infrastructure.Concurrency;
using MiniDocumentNotifier.WinForms.Forms;

namespace MiniDocumentNotifier.WinForms
{
    public class AppContext : ApplicationContext
    {
        private bool _isBackgroundAppRunning;

        public AppContext()
        {
            var splashScreen = new SplashScreenForm();
            splashScreen.FormClosed += SplashScreenForm_FormClosed;
            splashScreen.Show();
        }

        private void SplashScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isBackgroundAppRunning = CheckBackgroundAppRunning();

            var loginWizardForm = new LoginWizardForm();
            loginWizardForm.FormClosed += LoginWizardForm_FormClosed;
            loginWizardForm.Show();
        }

        private void LoginWizardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var mainForm = new MainForm(_isBackgroundAppRunning);
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