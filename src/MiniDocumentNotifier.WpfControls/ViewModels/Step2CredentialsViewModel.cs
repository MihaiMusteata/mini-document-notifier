using System.Windows.Input;
using MiniDocumentNotifier.WpfControls.Commands;

namespace MiniDocumentNotifier.WpfControls.ViewModels
{
    public class Step2CredentialsViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private bool _isPasswordVisible;

        public Step2CredentialsViewModel()
        {
            TogglePasswordVisibilityCommand = new RelayCommand(() => IsPasswordVisible = !IsPasswordVisible);
        }

        public ICommand TogglePasswordVisibilityCommand { get; }

        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                if (_isPasswordVisible == value) return;
                _isPasswordVisible = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PasswordToggleLabel));
            }
        }

        public string PasswordToggleLabel => _isPasswordVisible ? "Hide password" : "Show password";
    }
}