using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using autorent.Models;
using autorent.Stores;
using System.Windows.Input;
using autorent.Commands;
using autorent.Services;
using System.Windows;

namespace autorent.ViewModels
{
    public class AdminSalesViewModel : ViewModelBase
    {
        private readonly AccountStore _accountStore;
        private readonly WebsocketDataUpdateService _websocketDataUpdateService;
        public ICommand DeleteCommand { get; }
        public ICommand CreateCommand { get; }
        private List<Sale> _sales;
        private Sale _selectedSale;
        private Car _selectedCar;
        private int _newPercent;
        private string _newDescription;
        private bool _isTextBoxVisible = false;
        public List<Car> CarsList { get; set; }=new List<Car>();


        public bool IsTextBoxVisible
        {
            get => _isTextBoxVisible;
            set
            {
                _isTextBoxVisible = value;
                OnPropertyChanged(nameof(IsTextBoxVisible));
            }
        }
        public string NewDescription
        {
            get => _newDescription;
            set
            {
                _newDescription = value;
                OnPropertyChanged(nameof(NewDescription));
            }
        }
        public int NewPercent
        {
            get => _newPercent;
            set
            {
                _newPercent = value;
                OnPropertyChanged(nameof(NewPercent));
            }
        }
       public Car SelectedCar
        {
            get => _selectedCar;
            set
            {
                _selectedCar = value;
                OnPropertyChanged(nameof(SelectedCar));
            }
        }
        public Sale SelectedSale
        {
            get => _selectedSale;
            set
            {
                _selectedSale = value;
                OnPropertyChanged(nameof(SelectedSale));
            }
        }
        public List<Sale> TableData
        {
            get => _sales;
            set
            {
                _sales = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        public AdminSalesViewModel(AccountStore accountStore, WebsocketDataUpdateService websocketDataUpdateService)
        {
            _accountStore = accountStore;
            _websocketDataUpdateService = websocketDataUpdateService;
            _websocketDataUpdateService.OnSaleCreated += _websocketDataUpdateService_OnSaleCreated;
            _websocketDataUpdateService.OnSaleDeleted += _websocketDataUpdateService_OnSaleDeleted;
            _websocketDataUpdateService.OnCarCreated += _websocketDataUpdateService_OnCarCreated;
            _websocketDataUpdateService.OnCarEdited += _websocketDataUpdateService_OnCarEdited;
            _websocketDataUpdateService.OnCarDeleted += _websocketDataUpdateService_OnCarDeleted;

            UpdateData();
            DeleteCommand = new AdminSalesDeleteCommand(this, _accountStore);
            CreateCommand = new AdminSalesCreateCommand(this, _accountStore);
            GetCars();
        }

        public void UpdateData()
        {
            try
            {
                TableData = APICommunicationService.GetListOfObject<Sale>("/sales", _accountStore.CurrentAccount.Token);
                if (TableData == null)
                    TableData = new List<Sale>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void GetCars()
        {
            try
            {
                CarsList = APICommunicationService.GetListOfObject<Car>("/cars", _accountStore.CurrentAccount.Token);
                if (CarsList == null)
                    CarsList = new List<Car>();
                OnPropertyChanged(nameof(CarsList));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void _websocketDataUpdateService_OnSaleCreated(Sale sale)
        {
            try
            {
                TableData = new List<Sale>(TableData.Append(sale));
                OnPropertyChanged(nameof(TableData));
            }
            catch { }
        }
        private void _websocketDataUpdateService_OnSaleDeleted(int idOfDeletedItem)
        {
            try
            {
                Sale deleteThis = TableData.Where(x => x.Car.Id == idOfDeletedItem)?.First();
                if (deleteThis != null)
                {
                    TableData = new List<Sale>(TableData.Except(new List<Sale>() { deleteThis }));
                }
                OnPropertyChanged(nameof(TableData));
            }
            catch { }
        }

        private void _websocketDataUpdateService_OnCarCreated(Car car)
        {
            try
            {
                CarsList = new List<Car>(CarsList.Append(car));
                OnPropertyChanged(nameof(CarsList));
            }
            catch { }
        }
        private void _websocketDataUpdateService_OnCarEdited(Car car)
        {
            try
            {
                Car carToEdit = CarsList.Where(x => x.Id == car.Id)?.First();
                if (carToEdit != null)
                {
                    carToEdit.Category = car.Category;
                    carToEdit.Brand = car.Brand;
                    carToEdit.Model = car.Model;
                    carToEdit.DailyPrice = car.DailyPrice;
                    carToEdit.UnavailableDates = car.UnavailableDates;
                    carToEdit.DiscountPercentage = car.DiscountPercentage;
                }
                OnPropertyChanged(nameof(CarsList));
            }
            catch { }
        }
        private void _websocketDataUpdateService_OnCarDeleted(int idOfDeletedItem)
        {
            try
            {
                Car deleteThis = CarsList.Where(x => x.Id == idOfDeletedItem)?.First();
                if (deleteThis != null)
                {
                    CarsList = new List<Car>(CarsList.Except(new List<Car>() { deleteThis }));
                }
                OnPropertyChanged(nameof(CarsList));
            }
            catch { }
        }

        public override void Dispose()
        {
            _websocketDataUpdateService.OnSaleCreated -= _websocketDataUpdateService_OnSaleCreated;
            _websocketDataUpdateService.OnSaleDeleted -= _websocketDataUpdateService_OnSaleDeleted;
            _websocketDataUpdateService.OnCarCreated -= _websocketDataUpdateService_OnCarCreated;
            _websocketDataUpdateService.OnCarEdited -= _websocketDataUpdateService_OnCarEdited;
            _websocketDataUpdateService.OnCarDeleted -= _websocketDataUpdateService_OnCarDeleted;


            base.Dispose();
        }
    }
}
