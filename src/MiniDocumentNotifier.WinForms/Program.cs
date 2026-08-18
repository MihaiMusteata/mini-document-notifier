using System;
using System.Windows.Forms;
using MiniDocumentNotifier.Domain.Abstractions;
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

            var logger = Bootstrapper.Container.Resolve<ILogger>();

            using (var guard = Bootstrapper.Container.Resolve<ISingleInstanceGuard>())
            {
                if (!guard.TryAcquire())
                {
                    logger.Warning("WinForms startup blocked: another instance is already running.");
                    MessageBox.Show("An instance of MiniDocumentNotifier is already running", "Mini Document Notifier",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                logger.Info("WinForms application started: single instance acquired.");

                Application.Run(Bootstrapper.Container.Resolve<AppContext>());
            }
        }
    }
}