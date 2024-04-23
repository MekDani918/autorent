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
    public class AdminCategoriesDeleteCommand : CommandBase
    {
        private readonly AdminCategoriesViewModel _viewModel;
        private readonly AccountStore _accountStore;
        public AdminCategoriesDeleteCommand(AdminCategoriesViewModel viewModel, AccountStore accountStore)
        {
            _viewModel = viewModel;         
            _accountStore = accountStore;
        }

        public override void Execute(object? parameter)
        {
            if (_viewModel.SelectedCategory == null)
            {
                return;
            }
            HttpResponseMessage resp = APICommunicationService.Delete($"/categories/{_viewModel.SelectedCategory.Id}", _accountStore.CurrentAccount.Token);

            if (!resp.IsSuccessStatusCode)
            {
                ResponseWithMessage respObj = JsonSerializer.Deserialize<ResponseWithMessage>(resp.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                MessageBox.Show(respObj.message);
                
                return;
            }
            _viewModel.SelectedCategory = null;
            _viewModel.UpdateData();
        }
    }
}
