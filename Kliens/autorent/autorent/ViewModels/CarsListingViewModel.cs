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
        private readonly ObservableCollection<Category> _categoryList;
        public IEnumerable<CarsListingItemViewModel> CarsListingItemviewModels => _carsListingItemviewModels;
        public IEnumerable<Category> CategoryList => _categoryList;

        private readonly AccountStore _accountStore;
        private readonly SelectedCarStore _SelectedCarStore;

        public bool IsCategorySelected => SelectedCategory != null;
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                OnPropertyChanged(nameof(IsCategorySelected));
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
            _categoryList = new ObservableCollection<Category>();

            //_carsListingItemviewModels.Add(new CarsListingItemViewModel("Volkswagen", "golf", "Hatchback", 14990));
            //_carsListingItemviewModels.Add(new CarsListingItemViewModel("BMW", "E30", "Coupe", 19990));
            //_carsListingItemviewModels.Add(new CarsListingItemViewModel("Mercedes", "Barabus", "SUV", 9990));
            _accountStore = accountStore;
            _SelectedCarStore = selectedCarStore;

            UpdateCarsList();
            UpdateCategories();
            
        }

        private void UpdateCategories()
        {
            try
            {
                List<Category> cat = APICommunicationService.GetListOfObject<Category>("/categories", _accountStore.CurrentAccount.Token);
                if (cat == null)
                    cat = new List<Category>();

                _categoryList.Add(new Category() { Id=-1,Name=""});
                //SelectedCategory = _categoryList.Last();
                foreach (Category category in cat)
                {
                    _categoryList.Add(category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateCarsList()
        {
            _carsListingItemviewModels.Clear();
            try
            {
                string queryParams = "";
                //if (!string.IsNullOrEmpty(SelectedCategory)) queryParams = $"?category={SelectedCategory}";
                if (SelectedCategory?.Id >= 0) queryParams = $"?category={SelectedCategory.Name}";

                List<Car> cars = APICommunicationService.GetListOfObject<Car>("/cars" + queryParams, _accountStore.CurrentAccount.Token);
                if (cars == null)
                    cars = new List<Car>();

                foreach (Car car in cars)
                {
                    //TEST DISCOUNT FOR EVERY VW
                    if (car.Brand == "Volkswagen") car.DiscountPercentage = 5;
                    
                    _carsListingItemviewModels.Add(new CarsListingItemViewModel(car));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
