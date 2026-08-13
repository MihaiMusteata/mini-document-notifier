using System.Windows.Controls;
using MiniDocumentNotifier.WpfControls.ViewModels;

namespace MiniDocumentNotifier.WpfControls.Views
{
    public partial class Step2CredentialsView : UserControl
    {
        public Step2CredentialsView()
        {
            InitializeComponent();
        }

        private void PasswordInput_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is Step2CredentialsViewModel viewModel)
            {
                viewModel.Password = PasswordInput.Password;
            }
        }
    }
}