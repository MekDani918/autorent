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
    public class AdminCategoriesCreateCommand : CommandBase
    {
        private readonly AdminCategoriesViewModel _viewModel;
        private readonly AccountStore _accountStore;

        public AdminCategoriesCreateCommand(AdminCategoriesViewModel viewModel, AccountStore accountStore)
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
            if (String.IsNullOrEmpty(_viewModel.NewCategoryName))
            {
                return;
            }
            RequestBodyNewCategory requestBody = new RequestBodyNewCategory()
            {
                name = _viewModel.NewCategoryName
            };
            HttpResponseMessage resp = APICommunicationService.Post<RequestBodyNewCategory>("/categories", requestBody, _accountStore.CurrentAccount.Token);

            if (!resp.IsSuccessStatusCode)
            {
                ResponseWithMessage respObj = JsonSerializer.Deserialize<ResponseWithMessage>(resp.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                MessageBox.Show(respObj.message);
                return;
            }
            _viewModel.IsTextBoxVisible = false;
            _viewModel.NewCategoryName = "";
            _viewModel.UpdateData();
        }
    }
}
