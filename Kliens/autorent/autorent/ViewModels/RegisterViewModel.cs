using autorent.Commands;
using autorent.Models;
using autorent.Services;
using autorent.Stores;
using System.Windows.Input;
using System.Windows.Navigation;

namespace autorent.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand RegisterCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public RegisterViewModel(INavigationService<LoginViewModel> loginNavigationService)
        {
            RegisterCommand = new RegisterCommand(this, loginNavigationService);
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
        }
    }
}
