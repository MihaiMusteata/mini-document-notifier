using System;
using System.Windows.Forms;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly bool _isBackgroundAppRunning;
        
        public MainForm(bool isBackgroundAppRunning)
        {
            _isBackgroundAppRunning = isBackgroundAppRunning;
            InitializeComponent();
            MessageBox.Show(isBackgroundAppRunning ? "is running" : "is not running");
        }

    }
}