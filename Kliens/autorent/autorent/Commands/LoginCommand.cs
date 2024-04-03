using autorent.Models;
using autorent.Services;
using autorent.Stores;
using autorent.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace autorent.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _viewModel;
        private readonly INavigationService<CarsViewModel> _navigationService;
        private readonly AccountStore _accountStore;

        public LoginCommand(LoginViewModel viewModel, AccountStore accountStore, INavigationService<CarsViewModel> navigationService)
        {
            _viewModel = viewModel;
            _navigationService = navigationService;
            _accountStore = accountStore;
        }

        public override void Execute(object? parameter)
        {
            Account? account = loginUser();
            if(account == null)
            {
                MessageBox.Show("Hibás felhasználónév vagy jelszó!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _accountStore.CurrentAccount = account;

            _navigationService.Navigate();
        }

        private Account? loginUser()
        {
            //TODO Login logic here
            if(_viewModel.Username.StartsWith("user") && _viewModel.Password == "123")
            {
                return new Account(_viewModel.Username, "ezegytoken");
            }

            return null;
        }
    }
}
