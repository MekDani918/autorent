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
    public class AdminCarsSaveCommand : CommandBase
    {
        private readonly AdminCarsDetailsViewModel _viewModel;
        private readonly SelectedCarStore _selectedCarStore;
        private readonly AccountStore _accountStore;
        public AdminCarsSaveCommand(AdminCarsDetailsViewModel viewModel, SelectedCarStore selectedCarStore, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _selectedCarStore = selectedCarStore;
            _accountStore = accountStore;
        }

        public override void Execute(object? parameter)
        {
            //ÚJ AUTÓ
            if(_viewModel.EditedCar.Id == -1)
            {
                RequestBodyNewCar requestBody = new RequestBodyNewCar()
                {
                    brand = _viewModel.EditedCar.Brand,
                    model = _viewModel.EditedCar.Model,
                    categoryId = _viewModel.SelectedCategory.Id,
                    dailyPrice = _viewModel.EditedCar.DailyPrice
                };
                HttpResponseMessage resp = APICommunicationService.Post<RequestBodyNewCar>("/cars", requestBody, _accountStore.CurrentAccount.Token);

                if (!resp.IsSuccessStatusCode)
                {
                    ResponseWithMessage respObj = JsonSerializer.Deserialize<ResponseWithMessage>(resp.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    MessageBox.Show(respObj.message);
                    return;
                }

            }
            //AUTÓ MÓDOSÍTÁS
            else
            {
                RequestBodyNewCar requestBody = new RequestBodyNewCar()
                {
                    brand = _viewModel.EditedCar.Brand,
                    model = _viewModel.EditedCar.Model,
                    categoryId = _viewModel.SelectedCategory.Id,
                    dailyPrice = _viewModel.EditedCar.DailyPrice
                };

                HttpResponseMessage resp = APICommunicationService.Patch<RequestBodyNewCar>($"/cars/{_viewModel.EditedCar.Id}", requestBody, _accountStore.CurrentAccount.Token);

                if (!resp.IsSuccessStatusCode)
                {
                    ResponseWithMessage respObj = JsonSerializer.Deserialize<ResponseWithMessage>(resp.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    MessageBox.Show(respObj.message);
                    return;
                }
            }

            MessageBox.Show("Siker!");
            _viewModel.IsEditEnabled = !_viewModel.IsEditEnabled;
            _selectedCarStore.SelectedCar = null;
        }
    }
}
