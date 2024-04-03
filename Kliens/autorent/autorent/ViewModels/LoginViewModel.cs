using autorent.Commands;
using autorent.Models;
using autorent.Services;
using autorent.Stores;
using System.Windows.Input;
using System.Windows.Navigation;

namespace autorent.ViewModels
{
    public class LoginViewModel : ViewModelBase
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

        public ICommand LoginCommand { get; }
        public LoginViewModel(AccountStore accountStore, INavigationService<CarsViewModel> carsNavigationService)
        {
            LoginCommand = new LoginCommand(this, accountStore, carsNavigationService);
        }
    }
}
