using autorent.Models;
using autorent.Services;
using autorent.Stores;
using autorent.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace autorent.Commands
{
    public class AdminSalesDeleteCommand : CommandBase
    {
        private readonly AdminSalesViewModel _viewModel;
        private readonly AccountStore _accountStore;

        public AdminSalesDeleteCommand(AdminSalesViewModel viewModel, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
        }

        public override void Execute(object? parameter)
        {
            if (_viewModel.SelectedSale == null)
            {
                return;
            }
            HttpResponseMessage resp = APICommunicationService.Delete($"/sales/{_viewModel.SelectedSale.Car.Id}", _accountStore.CurrentAccount.Token);

            if (!resp.IsSuccessStatusCode)
            {
                ResponseWithMessage respObj = JsonSerializer.Deserialize<ResponseWithMessage>(resp.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                MessageBox.Show(respObj.message);

                return;
            }
            _viewModel.SelectedSale = null;
            _viewModel.UpdateData();
        }
    }
}
