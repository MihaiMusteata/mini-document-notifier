using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class SplashScreenForm : Form
    {
        public SplashScreenForm()
        {
            InitializeComponent();
            CenterProgressBar();
            Resize += (sender, e) => CenterProgressBar();
        }
        
        #region UI Settings
        private void CenterProgressBar()
        {
            progressBar.Width = panel.ClientSize.Width * 40 / 100;
            progressBar.Left = (panel.ClientSize.Width - progressBar.Width) / 2;
            progressBar.Top = (panel.ClientSize.Height - progressBar.Height) / 2;
        }
        #endregion


    }
}