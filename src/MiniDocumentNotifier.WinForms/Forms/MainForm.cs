using System;
using System.Configuration;
using System.Windows.Forms;
using MiniDocumentNotifier.Infrastructure.ViewConfiguration;

namespace MiniDocumentNotifier.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly bool _isBackgroundAppRunning;

        public MainForm(bool isBackgroundAppRunning)
        {
            InitializeComponent();
            _isBackgroundAppRunning = isBackgroundAppRunning;
            CheckConfiguration();
        }

        private void CheckConfiguration()
        {
            var stalenessThresholdHours =
                int.Parse(ConfigurationManager.AppSettings["ViewConfigStalenessThresholdHours"]);
            var viewConfigPath = ConfigurationManager.AppSettings["ViewConfigPath"];
            var store = new JsonViewConfigurationStore(TimeSpan.FromHours(stalenessThresholdHours), viewConfigPath);

            var result = store.Load();

            if (result.FileExists && !result.IsStale && _isBackgroundAppRunning)
                return;

            var message = !result.FileExists ? "Configuration file not found." :
                result.IsStale ? "Configuration file is stale." : "";

            if (!_isBackgroundAppRunning)
                message += " Background App is not running.";
            
            MessageBox.Show(this, message, "Configuration Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}