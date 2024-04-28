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
        public AdminSalesViewModel(AccountStore accountStore)
        {
            _accountStore = accountStore;
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
    }
}
