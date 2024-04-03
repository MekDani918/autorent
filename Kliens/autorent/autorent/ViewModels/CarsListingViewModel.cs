using autorent.Models;
using autorent.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using autorent.Stores;
using System.Windows.Navigation;
using System.Windows;
using System.Security.Principal;
using System.Text.Json.Serialization;
using System.Collections;

namespace autorent.ViewModels
{
    public class CarsListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<CarsListingItemViewModel> _carsListingItemviewModels;
        private readonly ObservableCollection<string> _categoryList;
        public IEnumerable<CarsListingItemViewModel> CarsListingItemviewModels => _carsListingItemviewModels;
        public IEnumerable<string> CategoryList => _categoryList;

        private readonly AccountStore _accountStore;
        private readonly SelectedCarStore _SelectedCarStore;

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                UpdateCarsList();
            }
        }

        private CarsListingItemViewModel _selectedCarsListingItemViewModel;
        public CarsListingItemViewModel SelectedCarsListingItemViewModel
        {
            get => _selectedCarsListingItemViewModel;
            set
            {
                _selectedCarsListingItemViewModel = value;
                OnPropertyChanged(nameof(SelectedCarsListingItemViewModel));

                _SelectedCarStore.SelectedCar = _selectedCarsListingItemViewModel?.Car;
            }
        }
        public Car SelectedCar => _SelectedCarStore.SelectedCar;

        public CarsListingViewModel(AccountStore accountStore, SelectedCarStore selectedCarStore)
        {
            _carsListingItemviewModels = new ObservableCollection<CarsListingItemViewModel>();
            _categoryList = new ObservableCollection<string>();

            //_carsListingItemviewModels.Add(new CarsListingItemViewModel("Volkswagen", "golf", "Hatchback", 14990));
            //_carsListingItemviewModels.Add(new CarsListingItemViewModel("BMW", "E30", "Coupe", 19990));
            //_carsListingItemviewModels.Add(new CarsListingItemViewModel("Mercedes", "Barabus", "SUV", 9990));
            _accountStore = accountStore;

            UpdateCarsList();

            try
            {
                List<Category> cat = getListOfData<Category>("/categories", _accountStore.CurrentAccount.Token);
                if (cat == null)
                    cat = new List<Category>();

                _categoryList.Add("");
                foreach (Category category in cat)
                {
                    _categoryList.Add(category.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            _SelectedCarStore = selectedCarStore;
        }
        public void UpdateCarsList()
        {
            _carsListingItemviewModels.Clear();
            try
            {
                string queryParams = "";
                if (!string.IsNullOrEmpty(SelectedCategory)) queryParams = $"?category={SelectedCategory}";

                List<Car> cars = getListOfData<Car>("/cars" + queryParams, _accountStore.CurrentAccount.Token);
                if (cars == null)
                    cars = new List<Car>();

                foreach (Car car in cars)
                {
                    _carsListingItemviewModels.Add(new CarsListingItemViewModel(car));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<T> getListOfData<T>(string endpoint, string token)
        {
            HttpResponseMessage resp = APICommunicationService.GetData(endpoint, token);

            if (!resp.IsSuccessStatusCode)
            {
                switch (resp.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new Exception($"Authentikációs hiba!");
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.InternalServerError:
                        throw new Exception($"Valami nem jó!");
                }
            }

            string respDataString = resp.Content.ReadAsStringAsync().Result;
            List<T> respData = JsonSerializer.Deserialize<List<T>>(respDataString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return respData;
        }
    }
}
