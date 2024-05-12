using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using autorent.Services;
using autorent.Stores;

namespace autorent.ViewModels
{
    public class CarsViewModel : ViewModelBase
    {
        public CarsListingViewModel CarsListingViewModel { get; }
        public CarsDetailsViewModel CarsDetailsViewModel { get; }

        public CarsViewModel(AccountStore accountStore, SelectedCarStore selectedCarStore, WebsocketDataUpdateService websocketDataUpdateService)
        {
            CarsListingViewModel = new CarsListingViewModel(accountStore, selectedCarStore, websocketDataUpdateService);
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
