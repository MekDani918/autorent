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
    public class RentCommand : CommandBase
    {
        private readonly CarsDetailsViewModel _viewModel;
        private readonly AccountStore _accountStore;

        public RentCommand(CarsDetailsViewModel viewModel, AccountStore accountStore)
        {
            _viewModel = viewModel;
            _accountStore = accountStore;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                rentCar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rentCar()
        {
            if (_viewModel.SelectedDateFrom == null || _viewModel.SelectedDateTo == null)
                throw new Exception("Hibás dátum");

            var postuser = new postkolcsonzes { carId = Convert.ToInt32(_viewModel.SelectedCar.Id), from = (((DateTimeOffset)_viewModel.SelectedDateFrom).ToUnixTimeSeconds()).ToString(), to = (((DateTimeOffset)_viewModel.SelectedDateTo).ToUnixTimeSeconds()).ToString() };


            var valasz = APICommunicationService.PostData<postkolcsonzes>("/rentals", postuser, _accountStore.CurrentAccount.Token);

            if (valasz.IsSuccessStatusCode)
            {
                MessageBox.Show("Sikeres kölcsönzés!");
            }
            else
            {
                switch (valasz.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new Exception($"Authentikációs hiba!");
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.InternalServerError:
                        throw new Exception($"Valami nem jó!");
                }
            }
        }
    }
}
