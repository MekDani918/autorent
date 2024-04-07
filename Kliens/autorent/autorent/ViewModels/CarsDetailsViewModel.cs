using autorent.Commands;
using autorent.Models;
using autorent.Stores;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using autorent.Services;

namespace autorent.ViewModels
{
    public class CarsDetailsViewModel : ViewModelBase
    {
        private readonly SelectedCarStore _selectedCarStore;
        private readonly AccountStore _accountStore;

        public ICommand RentCommand { get; }
        
        public ObservableCollection<CalendarDateRange> BlackoutDates { get; set; }
        public ObservableCollection<DateTime> SelectedDates { get; set; }
        public Car SelectedCar;

        public bool HasSelectedCar => SelectedCar != null;
        public string Brand => SelectedCar?.Brand ?? "";
        public string Model => SelectedCar?.Model ?? "";
        public string Category => SelectedCar?.Category ?? "";
        public int DailyPrice => SelectedCar?.DailyPrice ?? 0;

        private int _calculatedPriceSum = 0;
        private DateTime? _selectedDateFrom;
        private DateTime? _selectedDateTo;
        private bool _isRentButtonEnabled = false;

        public int CalculatedPriceSum
        {
            get => _calculatedPriceSum;
            set
            {
                _calculatedPriceSum = value;
                OnPropertyChanged(nameof(CalculatedPriceSum));
            }
        }
        public DateTime? SelectedDateFrom
        {
            get => _selectedDateFrom;
            set
            {
                _selectedDateFrom = value;
                updatePrice();
            }
        }
        public DateTime? SelectedDateTo
        {
            get => _selectedDateTo;
            set
            {
                _selectedDateTo = value;
                updatePrice();
            }
        }
        public bool IsRentButtonEnabled
        {
            get => _isRentButtonEnabled;
            set
            {
                _isRentButtonEnabled = value;
                OnPropertyChanged(nameof(IsRentButtonEnabled));
            }
        }

        public CarsDetailsViewModel(SelectedCarStore selectedCarStore, AccountStore accountStore)
        {
            BlackoutDates = new ObservableCollection<CalendarDateRange>();
            SelectedDates = new ObservableCollection<DateTime>();

            _selectedCarStore = selectedCarStore;
            _accountStore = accountStore;

            updateSelectedCar();

            RentCommand = new RentCommand(this, accountStore);
            ((RentCommand)RentCommand).RentalSuccessful += OnRentalSuccessful;

            _selectedCarStore.SelectedCarChanged += OnSelectedCarChanged;

            updateBlackoutDates();
        }

        private void OnSelectedCarChanged()
        {
            updateSelectedCar();

            OnPropertyChanged(nameof(HasSelectedCar));
            OnPropertyChanged(nameof(Brand));
            OnPropertyChanged(nameof(Model));
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(DailyPrice));

            SelectedDateFrom = null;
            SelectedDateTo = null;
            OnPropertyChanged(nameof(SelectedDateFrom));
            OnPropertyChanged(nameof(SelectedDateTo));

            updateBlackoutDates();
            updateSelectedDatesOnCalendar();
        }

        private void updateBlackoutDates()
        {
            BlackoutDates.Clear();
            OnPropertyChanged(nameof(BlackoutDates));
            if (SelectedCar != null)
            {
                foreach (string date in SelectedCar.UnavailableDates)
                {
                    BlackoutDates.Add(new CalendarDateRange(DateTime.Parse(date)));
                }
            }
            BlackoutDates = new ObservableCollection<CalendarDateRange>(BlackoutDates);
            OnPropertyChanged(nameof(BlackoutDates));
        }
        private void updateSelectedDatesOnCalendar()
        {
            SelectedDates.Clear();
            if (!(SelectedCar == null || SelectedDateFrom == null || SelectedDateTo == null))
            {
                for (DateTime i = SelectedDateFrom.Value; i <= SelectedDateTo.Value; i = i.AddDays(1))
                {
                    SelectedDates.Add(i);
                }
            }
            SelectedDates = new ObservableCollection<DateTime>(SelectedDates);
            OnPropertyChanged(nameof(SelectedDates));
        }

        private void updatePrice()
        {
            if (SelectedDateFrom == null || SelectedDateTo == null)
            {
                CalculatedPriceSum = 0;
                IsRentButtonEnabled = false;
            }
            else if (SelectedDateFrom > SelectedDateTo)
            {
                MessageBox.Show("Nem lehet a kezdődátum nagyobb mint a végdátum", "Dátumkiválasztási hiba");
                SelectedDateTo = null;
                IsRentButtonEnabled = false;
            }
            else if (SelectedDateFrom < DateTime.Now.Date || SelectedDateTo < DateTime.Now.Date)
            {
                MessageBox.Show("Múltbéli időre foglalás nem lehetséges", "Dátumkiválasztási hiba");
                SelectedDateFrom = null;
                SelectedDateTo = null;
                IsRentButtonEnabled = false;
            }
            else if (CheckIfUnavailable())
            {
                MessageBox.Show("Az autó már foglalt a kijelölt időszakban", "Dátumkiválasztási hiba");
                SelectedDateTo = null;
                IsRentButtonEnabled = false;
            }
            else
            {
                int days = (SelectedDateTo - SelectedDateFrom).Value.Days + 1;
                CalculatedPriceSum = (int)Math.Round(days * SelectedCar.DailyPrice * (100 - SelectedCar.DiscountPercentage) / 100);
                IsRentButtonEnabled = true;
                updateSelectedDatesOnCalendar();
            }
        }
        private void updateSelectedCar()
        {
            if (_selectedCarStore.SelectedCar == null)
            {
                SelectedCar = null;
            }
            else
            {
                SelectedCar = APICommunicationService.GetObject<Car>($"/cars/{_selectedCarStore.SelectedCar.Id}", _accountStore.CurrentAccount.Token);
            }
            OnPropertyChanged(nameof(SelectedCar));
        }

        private bool CheckIfUnavailable()
        {
            foreach(CalendarDateRange drange in BlackoutDates)
            {
                if(
                    (drange.Start >= SelectedDateFrom && drange.Start <= SelectedDateTo) &&
                    (drange.End >= SelectedDateFrom && drange.End <= SelectedDateTo)
                )
                {
                    return true;
                }
            }
            return false;
        }

        private void OnRentalSuccessful()
        {
            OnSelectedCarChanged();
        }

        public override void Dispose()
        {
            _selectedCarStore.SelectedCarChanged -= OnSelectedCarChanged;
            ((RentCommand)RentCommand).RentalSuccessful -= OnRentalSuccessful;
            _selectedCarStore.SelectedCar = null;

            base.Dispose();
        }
    }
}
