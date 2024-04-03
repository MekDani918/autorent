using autorent.Services;
using autorent.Stores;
using autorent.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Commands
{
    public class LogoutCommand : CommandBase
    {
        private readonly AccountStore _accountStore;
        private readonly INavigationService<LoginViewModel> _navigationService;

        public LogoutCommand(AccountStore accountStore, INavigationService<LoginViewModel> navigationService)
        {
            _accountStore = accountStore;
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _accountStore.Logout();

            _navigationService.Navigate();
        }
    }
}
