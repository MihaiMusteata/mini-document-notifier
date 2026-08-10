using System.Windows.Forms;
using MiniDocumentNotifier.WinForms.Forms;

namespace MiniDocumentNotifier.WinForms
{
    public class AppContext : ApplicationContext
    {
        public AppContext()
        {
            var splashScreen = new SplashScreenForm();
            splashScreen.FormClosed += SplashScreenForm_FormClosed;
            splashScreen.Show();
        }

        private void SplashScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var loginWizardForm = new LoginWizardForm();
            loginWizardForm.FormClosed += LoginWizardForm_FormClosed;
            loginWizardForm.Show();
        }

        private void LoginWizardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var mainForm = new MainForm();
            mainForm.FormClosed += (s, args) =>
            {
                ExitThread();
            };
            mainForm.Show();
        }
        
    }
}