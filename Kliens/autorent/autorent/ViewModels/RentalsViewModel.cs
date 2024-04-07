using autorent.Models;
using autorent.Services;
using autorent.Stores;
using System.Data;
using System.Windows;

namespace autorent.ViewModels
{
    public class RentalsViewModel : ViewModelBase
    {
        private readonly AccountStore _accountStore;

        private List<Rental> _rentals;
        public List<Rental> TableData
        {
            get => _rentals;
            set
            {
                _rentals = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        public RentalsViewModel(AccountStore accountStore)
        {
            _accountStore = accountStore;

            updateData();
        }

        private void updateData()
        {
            try
            {
                TableData = APICommunicationService.GetListOfObject<Rental>("/rentals", _accountStore.CurrentAccount.Token);
                if (TableData == null)
                    TableData = new List<Rental>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
