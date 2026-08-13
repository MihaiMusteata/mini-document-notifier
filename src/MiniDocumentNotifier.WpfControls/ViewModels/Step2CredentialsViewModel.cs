namespace MiniDocumentNotifier.WpfControls.ViewModels
{
    public class Step2CredentialsViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private bool _isPasswordVisible;

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
            set => SetField(ref _isPasswordVisible, value);
        }

    }
}