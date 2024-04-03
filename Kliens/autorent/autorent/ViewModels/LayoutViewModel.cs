using autorent.Commands;
using autorent.Stores;
using autorent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using autorent.Models;

namespace autorent.ViewModels
{
    public class LayoutViewModel :ViewModelBase
    {
        public NavigationBarViewModel NavigationBarViewModel { get; set; }
        public ViewModelBase ContentViewModel { get; }

        //AccountStore _accountStore;

        //public string Username => _accountStore.CurrentAccount?.Username;


        //public ICommand NavigateLoginCommand { get; }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
        }

        public override void Dispose()
        {
            NavigationBarViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }

        //public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, AccountStore accountStore, NavigationService<LoginViewModel> loginNavigationService)
        //{
        //    NavigationBarViewModel = navigationBarViewModel;
        //    _accountStore = accountStore;
        //    NavigateLoginCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
        //}
    }
}
