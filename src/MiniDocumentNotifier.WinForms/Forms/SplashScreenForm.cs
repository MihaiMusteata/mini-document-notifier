using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class SplashScreenForm : Form
    {
        public event Func<Task> InitializationSteps;
        
        public SplashScreenForm()
        {
            InitializeComponent();
            LayoutSplashControls();
            Resize += (sender, e) => LayoutSplashControls();
            Shown += async (s, e) => await RunInitializationAsync();
        }

        private async Task RunInitializationAsync()
        {
            if (InitializationSteps != null)
                await InitializationSteps.Invoke();

            Close();
        }
        
        public void SetStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => lblStatus.Text = message));
            }
            else
            {
                lblStatus.Text = message;
            }
        }


        #region UI Settings
        private void LayoutSplashControls()
        {
            progressBar.Width = panel.ClientSize.Width * 40 / 100;
            progressBar.Left = (panel.ClientSize.Width - progressBar.Width) / 2;

            const int spacing = 6;
            var groupHeight = lblStatus.Height + spacing + progressBar.Height;
            var groupTop = (panel.ClientSize.Height - groupHeight) / 2;

            lblStatus.Left = progressBar.Left;
            lblStatus.Width = progressBar.Width;
            lblStatus.Top = groupTop;

            progressBar.Top = lblStatus.Bottom + spacing;
        }
        #endregion


    }
}