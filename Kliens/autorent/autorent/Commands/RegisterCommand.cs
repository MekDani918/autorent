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
    public class RegisterCommand : CommandBase
    {
        private readonly RegisterViewModel _viewModel;
        private readonly INavigationService<LoginViewModel> _loginNavigationService;

        public RegisterCommand(RegisterViewModel viewModel, INavigationService<LoginViewModel> loginNavigationService)
        {
            _viewModel = viewModel;
            _loginNavigationService = loginNavigationService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                if (String.IsNullOrEmpty(_viewModel.Username) || String.IsNullOrEmpty(_viewModel.Password))
                {
                    MessageBox.Show("Adj meg felhasználónevet és jelszót!");
                    return;
                }
                RequestBodyRegister requestBody = new RequestBodyRegister()
                {
                    username = _viewModel.Username,
                    name = _viewModel.Name,
                    password = _viewModel.Password
                };
                HttpResponseMessage resp = APICommunicationService.Post<RequestBodyRegister>("/register", requestBody);

                if (!resp.IsSuccessStatusCode)
                {
                    ResponseWithMessage respObj = JsonSerializer.Deserialize<ResponseWithMessage>(resp.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    MessageBox.Show(respObj.message);
                    return;
                }

                _loginNavigationService.Navigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
