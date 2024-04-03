using autorent.Services;
using autorent.Stores;
using autorent.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace autorent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly AccountStore _accountStore;
        private readonly NavigationStore _navigationStore;

        public App()
        {
            _accountStore = new AccountStore();
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService<LoginViewModel> loginNavigationService = CreateLoginNavigationService();
            loginNavigationService.Navigate();

            //MainWindow = new MainWindow();
            MainWindow = new MainWindow2()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private INavigationService<LoginViewModel> CreateLoginNavigationService()
        {
            return new NavigationService<LoginViewModel>(_navigationStore, () => new LoginViewModel(_accountStore, CreateCarsNavigationService()));
        }
        //private INavigationService<LayoutViewModel> CreateLayoutViewNavigationService()
        //{
        //    return new NavigationService<LayoutViewModel>(_navigationStore, () => new LayoutViewModel(_navigationBarViewModel, CreateLoginNavigationService() ));
        //}

        private INavigationService<RentalsViewModel> CreateRentalsNavigationService()
        {
            return new LayoutNavigationService<RentalsViewModel>(
                _navigationStore,
                () => new RentalsViewModel(),
                CreateNavigationBarViewModel);
        }

        private INavigationService<CarsViewModel> CreateCarsNavigationService()
        {
            return new LayoutNavigationService<CarsViewModel>(
                _navigationStore,
                () => new CarsViewModel(),
                CreateNavigationBarViewModel);
        }

        private NavigationBarViewModel CreateNavigationBarViewModel()
        {
            return new NavigationBarViewModel(
                CreateCarsNavigationService(),
                CreateRentalsNavigationService(),
                CreateLoginNavigationService(),
                _accountStore);
        }
    }

}
