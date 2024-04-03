using autorent.Commands;
using autorent.Models;
using autorent.Stores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace autorent.ViewModels
{
    public class CarsDetailsViewModel : ViewModelBase
    {
        private readonly SelectedCarStore _selectedCarStore;

        public Car SelectedCar => _selectedCarStore.SelectedCar;
        
        public bool HasSelectedCar => SelectedCar != null;
        public string Brand => SelectedCar?.Brand ?? "";
        public string Model => SelectedCar?.Model ?? "";
        public string Category => SelectedCar?.Category ?? "";
        public int DailyPrice => SelectedCar?.DailyPrice ?? 0;

        private int _calculatedPriceSum = 0;
        public int CalculatedPriceSum
        {
            get => _calculatedPriceSum;
            set
            {
                _calculatedPriceSum = value;
                OnPropertyChanged(nameof(CalculatedPriceSum));
            }
        }
        private DateTime? _selectedDateFrom;
        public DateTime? SelectedDateFrom
        {
            get => _selectedDateFrom;
            set
            {
                _selectedDateFrom = value;
                updatePrice();
            }
        }
        private DateTime? _selectedDateTo;
        public DateTime? SelectedDateTo
        {
            get => _selectedDateTo;
            set
            {
                _selectedDateTo = value;
                updatePrice();
            }
        }
        private bool _isRentButtonEnabled = false;
        public bool IsRentButtonEnabled
        {
            get => _isRentButtonEnabled;
            set
            {
                _isRentButtonEnabled = value;
                OnPropertyChanged(nameof(IsRentButtonEnabled));
            }
        }

        public ICommand RentCommand { get; }


        public CarsDetailsViewModel(SelectedCarStore selectedCarStore, AccountStore accountStore)
        {
            _selectedCarStore = selectedCarStore;

            RentCommand = new RentCommand(this, accountStore);

            _selectedCarStore.SelectedCarChanged += SelectedCarStore_SelectedCarChanged;
        }

        private void SelectedCarStore_SelectedCarChanged()
        {
            OnPropertyChanged(nameof(HasSelectedCar));
            OnPropertyChanged(nameof(Brand));
            OnPropertyChanged(nameof(Model));
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(DailyPrice));
        }

        private void updateCalendars()
        {
            //TODO
            //Update calendars with current blackout dates
        }

        private void updatePrice()
        {
            if (SelectedDateFrom > SelectedDateTo)
            {
                MessageBox.Show("Nem lehet a kezdődátum nagyobb mint a végdátum", "Dátumkiválasztási hiba");
                SelectedDateTo = null;
                IsRentButtonEnabled = false;
            }
            else if (SelectedDateFrom == null || SelectedDateTo == null)
            {
                CalculatedPriceSum = 0;
                IsRentButtonEnabled = false;
            }
            else if (SelectedDateFrom == SelectedDateTo)
            {
                CalculatedPriceSum = SelectedCar.DailyPrice;
                IsRentButtonEnabled = true;
            }
            else
            {
                int days = (SelectedDateTo - SelectedDateFrom).Value.Days + 1;
                CalculatedPriceSum = days * SelectedCar.DailyPrice;
                IsRentButtonEnabled = true;
            }
        }

        ~CarsDetailsViewModel()
        {

        }

        public override void Dispose()
        {
            _selectedCarStore.SelectedCarChanged -= SelectedCarStore_SelectedCarChanged;
            _selectedCarStore.SelectedCar = null;

            base.Dispose();
        }
    }
}
