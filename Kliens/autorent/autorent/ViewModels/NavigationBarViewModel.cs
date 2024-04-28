using autorent.Commands;
using autorent.Services;
using autorent.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace autorent.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        public ICommand NavigateCarsCommand { get; }
        public ICommand NavigateRentalsCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateAdminCategoriesCommand { get; }
        public ICommand NavigateAdminCarsCommand { get; }
        public ICommand NavigateAdminSalesCommand { get; }
        public ICommand LogoutCommand { get; }
        private AccountStore _accountStore { get; set; }
        public string Role => _accountStore.CurrentAccount?.Role;


        public NavigationBarViewModel(
            INavigationService<CarsViewModel> carsNavigationService,
            INavigationService<RentalsViewModel> rentalsNavigationService,

            INavigationService<AdminCategoriesViewModel> adminCategoriesNavigationService,
            INavigationService<AdminCarsViewModel> adminCarsNavigationService,
            INavigationService<AdminSalesViewModel> adminSalesNavigationService,
            
            INavigationService<LoginViewModel> loginNavigationService,
            AccountStore accountStore)
        {
            NavigateCarsCommand = new NavigateCommand<CarsViewModel>(carsNavigationService);
            NavigateRentalsCommand = new NavigateCommand<RentalsViewModel>(rentalsNavigationService);

            NavigateAdminCategoriesCommand = new NavigateCommand<AdminCategoriesViewModel>(adminCategoriesNavigationService);
            NavigateAdminCarsCommand = new NavigateCommand<AdminCarsViewModel>(adminCarsNavigationService); ;
            NavigateAdminSalesCommand = new NavigateCommand<AdminSalesViewModel>(adminSalesNavigationService); ;
            
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
            LogoutCommand = new LogoutCommand(accountStore, loginNavigationService);

            _accountStore = accountStore;
        }

    }
}
