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
using System.Runtime.ConstrainedExecution;

namespace autorent.ViewModels
{
    public class CarsListingViewModel : ViewModelBase
    {
        private readonly AccountStore _accountStore;
        private readonly SelectedCarStore _SelectedCarStore;
        private readonly WebsocketDataUpdateService _websocketDataUpdateService;


        private ObservableCollection<CarsListingItemViewModel> _carsListingItemviewModels;
        private ObservableCollection<Category> _categoryList;
        public IEnumerable<CarsListingItemViewModel> CarsListingItemviewModels => _carsListingItemviewModels;
        public IEnumerable<Category> CategoryList => _categoryList;


        public Car SelectedCar => _SelectedCarStore.SelectedCar;
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

        public CarsListingViewModel(AccountStore accountStore, SelectedCarStore selectedCarStore, WebsocketDataUpdateService websocketDataUpdateService)
        {
            _carsListingItemviewModels = new ObservableCollection<CarsListingItemViewModel>();
            _categoryList = new ObservableCollection<Category>();

            _accountStore = accountStore;
            _SelectedCarStore = selectedCarStore;
            _websocketDataUpdateService = websocketDataUpdateService;
            _websocketDataUpdateService.OnCarCreated += _websocketDataUpdateService_OnCarCreated;
            _websocketDataUpdateService.OnCarEdited += _websocketDataUpdateService_OnCarEdited;
            _websocketDataUpdateService.OnCarDeleted += _websocketDataUpdateService_OnCarDeleted;
            _websocketDataUpdateService.OnSaleCreated += _websocketDataUpdateService_OnSaleCreated;
            _websocketDataUpdateService.OnSaleDeleted += _websocketDataUpdateService_OnSaleDeleted;
            _websocketDataUpdateService.OnCategoryCreated += _websocketDataUpdateService_OnCategoryCreated;
            _websocketDataUpdateService.OnCategoryDeleted += _websocketDataUpdateService_OnCategoryDeleted;

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
                if (SelectedCategory?.Id >= 0) queryParams = $"?category={SelectedCategory.Name}";

                List<Car> cars = APICommunicationService.GetListOfObject<Car>("/cars" + queryParams, _accountStore.CurrentAccount.Token);
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

        //WEBSOCKET UPDATE METHODS
        private void _websocketDataUpdateService_OnCarCreated(Car car)
        {
            try
            {
                _carsListingItemviewModels = new ObservableCollection<CarsListingItemViewModel>(CarsListingItemviewModels.Append(new CarsListingItemViewModel(car)));
                OnPropertyChanged(nameof(CarsListingItemviewModels));
            }
            catch { }
        }
        private void _websocketDataUpdateService_OnCarEdited(Car car)
        {
            try
            {
                CarsListingItemViewModel carToEdit = _carsListingItemviewModels.Where(x => x.Car.Id == car.Id)?.First();
                if (carToEdit != null)
                {
                    carToEdit.Category = car.Category;
                    carToEdit.Brand = car.Brand;
                    carToEdit.Model = car.Model;
                    carToEdit.Price = car.DailyPrice;
                    carToEdit.Car.UnavailableDates = car.UnavailableDates;
                    carToEdit.DiscountPercentage = car.DiscountPercentage;
                }
                OnPropertyChanged(nameof(CarsListingItemviewModels));
            }
            catch { }
        }
        private void _websocketDataUpdateService_OnCarDeleted(int idOfDeletedItem)
        {
            try
            {
                CarsListingItemViewModel deleteThis = _carsListingItemviewModels.Where(x => x.Car.Id == idOfDeletedItem)?.First();
                if (deleteThis != null)
                {
                    _carsListingItemviewModels = new ObservableCollection<CarsListingItemViewModel>(CarsListingItemviewModels.Except(new ObservableCollection<CarsListingItemViewModel>() { deleteThis }));
                }
                OnPropertyChanged(nameof(CarsListingItemviewModels));
            }
            catch { }
        }
        private void _websocketDataUpdateService_OnSaleCreated(Sale sale)
        {
            try
            {
                CarsListingItemViewModel carToEdit = _carsListingItemviewModels.Where(x => x.Car.Id == sale.Car.Id)?.First();
                if (carToEdit != null)
                {
                    carToEdit.DiscountPercentage = sale.Percent;
                }
                OnPropertyChanged(nameof(CarsListingItemviewModels));
            }
            catch { }
            MessageBox.Show($"Új akció érhető el a(z) {sale.Car.Brand} {sale.Car.Model} autóra!", "Új akció!", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
        }
        private void _websocketDataUpdateService_OnSaleDeleted(int idOfDeletedItem)
        {
            try
            {
                CarsListingItemViewModel carToEdit = _carsListingItemviewModels.Where(x => x.Car.Id == idOfDeletedItem)?.First();
                if (carToEdit != null)
                {
                    carToEdit.DiscountPercentage = 0;
                }
                OnPropertyChanged(nameof(CarsListingItemviewModels));
            }
            catch { }
        }
        private void _websocketDataUpdateService_OnCategoryCreated(Category category)
        {
            try
            {
                _categoryList = new ObservableCollection<Category>(CategoryList.Append(category));
                OnPropertyChanged(nameof(CategoryList));
            }
            catch { }
        }
        private void _websocketDataUpdateService_OnCategoryDeleted(int idOfDeletedItem)
        {
            try
            {
                Category deleteThis = _categoryList.Where(x => x.Id == idOfDeletedItem)?.First();
                if (deleteThis != null)
                {
                    _categoryList = new ObservableCollection<Category>(CategoryList.Except(new ObservableCollection<Category>() { deleteThis }));
                }
                OnPropertyChanged(nameof(CategoryList));
            }
            catch { }
        }

        public override void Dispose()
        {
            _websocketDataUpdateService.OnCarCreated -= _websocketDataUpdateService_OnCarCreated;
            _websocketDataUpdateService.OnCarEdited -= _websocketDataUpdateService_OnCarEdited;
            _websocketDataUpdateService.OnCarDeleted -= _websocketDataUpdateService_OnCarDeleted;
            _websocketDataUpdateService.OnSaleCreated -= _websocketDataUpdateService_OnSaleCreated;
            _websocketDataUpdateService.OnSaleDeleted -= _websocketDataUpdateService_OnSaleDeleted;
            _websocketDataUpdateService.OnCategoryCreated -= _websocketDataUpdateService_OnCategoryCreated;
            _websocketDataUpdateService.OnCategoryDeleted -= _websocketDataUpdateService_OnCategoryDeleted;

            base.Dispose();
        }
    }
}
