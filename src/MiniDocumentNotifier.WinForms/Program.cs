using System;
using System.Windows.Forms;
using MiniDocumentNotifier.Infrastructure.Concurrency;
using MiniDocumentNotifier.WinForms.Forms;

namespace MiniDocumentNotifier.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var guard = new MutexSingleInstanceGuard(Constants.WinFormsMutexName))
            {
                if (!guard.TryAcquire())
                {
                    MessageBox.Show("An instance of MiniDocumentNotifier is already running", "Mini Document Notifier",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Application.Run(new AppContext());
            }
        }
    }
}