using System;
using System.Windows.Forms;
using MiniDocumentNotifier.Infrastructure.Concurrency;
using MiniDocumentNotifier.WinForms.UnityBootstrapper;
using Unity;

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

            using (var guard = Bootstrapper.Container.Resolve<ISingleInstanceGuard>())
            {
                if (!guard.TryAcquire())
                {
                    MessageBox.Show("An instance of MiniDocumentNotifier is already running", "Mini Document Notifier",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Application.Run(Bootstrapper.Container.Resolve<AppContext>());
            }
        }
    }
}