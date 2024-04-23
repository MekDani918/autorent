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
    public class AdminCarsDeleteCommand : CommandBase
    {
        private readonly AdminCarsDetailsViewModel _viewModel;
        private readonly SelectedCarStore _selectedCarStore;
        private readonly AccountStore _accountStore;
        public AdminCarsDeleteCommand(AdminCarsDetailsViewModel viewModel, SelectedCarStore selectedCarStore, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _selectedCarStore = selectedCarStore;
            _accountStore = accountStore;
        }

        public override void Execute(object? parameter)
        {
            if (_viewModel.SelectedCar.Id != -1)
            {
                HttpResponseMessage resp = APICommunicationService.Delete($"/cars/{_viewModel.EditedCar.Id}", _accountStore.CurrentAccount.Token);

                if (!resp.IsSuccessStatusCode)
                {
                    ResponseWithMessage respObj = JsonSerializer.Deserialize<ResponseWithMessage>(resp.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    MessageBox.Show(respObj.message);
                    return;
                }
            }

            _selectedCarStore.SelectedCar = null;
        }
    }
}
