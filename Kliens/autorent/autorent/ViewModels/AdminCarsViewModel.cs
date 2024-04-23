using autorent.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.ViewModels
{
    public class AdminCarsViewModel : ViewModelBase
    {
        public CarsListingViewModel CarsListingViewModel { get; }
        public AdminCarsDetailsViewModel AdminCarsDetailsViewModel { get; }

        public AdminCarsViewModel(AccountStore accountStore, SelectedCarStore selectedCarStore)
        {
            CarsListingViewModel = new CarsListingViewModel(accountStore, selectedCarStore);
            AdminCarsDetailsViewModel = new AdminCarsDetailsViewModel(selectedCarStore, accountStore);
        }
        public override void Dispose()
        {
            CarsListingViewModel.Dispose();
            AdminCarsDetailsViewModel.Dispose();

            base.Dispose();
        }
    }
}
