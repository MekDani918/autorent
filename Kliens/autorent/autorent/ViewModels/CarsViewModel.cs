using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using autorent.Stores;

namespace autorent.ViewModels
{
    public class CarsViewModel : ViewModelBase
    {
        public CarsListingViewModel CarsListingViewModel { get; }
        public CarsDetailsViewModel CarsDetailsViewModel { get; }

        public CarsViewModel(AccountStore accountStore, SelectedCarStore selectedCarStore)
        {
            CarsListingViewModel = new CarsListingViewModel(accountStore, selectedCarStore);
            CarsDetailsViewModel = new CarsDetailsViewModel(selectedCarStore, accountStore);
        }
        public override void Dispose()
        {
            CarsListingViewModel.Dispose();
            CarsDetailsViewModel.Dispose();

            base.Dispose();
        }
    }
}
