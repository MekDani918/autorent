using autorent.Commands;
using autorent.Services;
using autorent.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace autorent.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        public ICommand NavigateCarsCommand { get; }
        public ICommand NavigateRentalsCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand LogoutCommand { get; }

        public NavigationBarViewModel(
            INavigationService<CarsViewModel> carsNavigationService,
            INavigationService<RentalsViewModel> rentalsNavigationService,
            INavigationService<LoginViewModel> loginNavigationService,
            AccountStore accountStore)
        {
            NavigateCarsCommand = new NavigateCommand<CarsViewModel>(carsNavigationService);
            NavigateRentalsCommand = new NavigateCommand<RentalsViewModel>(rentalsNavigationService);
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
            LogoutCommand = new LogoutCommand(accountStore, loginNavigationService);
        }
    }
}
