using autorent.Models;
using autorent.Services;
using autorent.Stores;
using autorent.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace autorent.Commands
{
    public class AdminSalesCreateCommand : CommandBase
    {
        private readonly AdminSalesViewModel _viewModel;
        private readonly AccountStore _accountStore;

        public AdminSalesCreateCommand(AdminSalesViewModel viewModel, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
        }

        public override void Execute(object? parameter)
        {
            if (_viewModel.IsTextBoxVisible == false)
            {
                _viewModel.IsTextBoxVisible = true;
                return;
            }
            if (String.IsNullOrEmpty(_viewModel.NewDescription)||_viewModel.SelectedCar==null||_viewModel.NewPercent==0)
            {
                return;
            }
            RequestBodyNewSale requestBody = new RequestBodyNewSale()
            {
                carId = _viewModel.SelectedCar.Id,
                description = _viewModel.NewDescription,
                percent = _viewModel.NewPercent
            };
            HttpResponseMessage resp = APICommunicationService.Post<RequestBodyNewSale>("/sales", requestBody, _accountStore.CurrentAccount.Token);
            
            if (!resp.IsSuccessStatusCode)
            {
                
                if (resp.StatusCode == HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("Az autó már le van árazva!");
                    return;
                }
                ResponseWithMessage respObj = JsonSerializer.Deserialize<ResponseWithMessage>(resp.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                MessageBox.Show(respObj.message);
                return;
            }
            _viewModel.IsTextBoxVisible = false;
            _viewModel.NewPercent = 0;
            _viewModel.NewDescription = "";
            _viewModel.SelectedCar = null;
            _viewModel.UpdateData();
        }
    }
}
