﻿using autorent.Models;
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
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _viewModel;
        private readonly AccountStore _accountStore;
        private readonly INavigationService<CarsViewModel> _carsNavigationService;
        private readonly INavigationService<AdminCategoriesViewModel> _adminCategoriesNavigationService;

        public LoginCommand(LoginViewModel viewModel, AccountStore accountStore, INavigationService<CarsViewModel> carsNavigationService, INavigationService<AdminCategoriesViewModel> adminCategoriesNavigationService)
        {
            _viewModel = viewModel;
            _carsNavigationService = carsNavigationService;
            _adminCategoriesNavigationService = adminCategoriesNavigationService;
            _accountStore = accountStore;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                Account? account = loginUser();
                if (account == null)
                    throw new Exception("Hibás felhasználónév vagy jelszó!");

                _accountStore.CurrentAccount = account;

                if(account.Role == "admin")
                    _adminCategoriesNavigationService.Navigate();
                else
                    _carsNavigationService.Navigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Account? loginUser()
        {
            if (string.IsNullOrEmpty(_viewModel.Username) || string.IsNullOrEmpty(_viewModel.Password))
                throw new Exception("Kérjük Töltse ki a mezőket!");


            var postuser = new userpost { username = _viewModel.Username, password = _viewModel.Password };
            HttpResponseMessage resp = APICommunicationService.Post<userpost>("/login", postuser);

            if (!resp.IsSuccessStatusCode)
            {
                switch (resp.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new Exception($"Hibás felhasználónév vagy jelszó!");
                    case HttpStatusCode.InternalServerError:
                        throw new Exception($"Valami nem jó!");
                }
            }
                
            string respDataString = resp.Content.ReadAsStringAsync().Result;
            userresponse respData = JsonSerializer.Deserialize<userresponse>(respDataString);
                
            string token = respData.token;
            string message = respData.message;
            
            return new Account(_viewModel.Username, token);
        }
    }
}
