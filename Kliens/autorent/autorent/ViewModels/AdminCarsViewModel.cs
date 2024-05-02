using autorent.Services;
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

        public AdminCarsViewModel(AccountStore accountStore, SelectedCarStore selectedCarStore, WebsocketDataUpdateService websocketDataUpdateService)
        {
            CarsListingViewModel = new CarsListingViewModel(accountStore, selectedCarStore, websocketDataUpdateService);
            AdminCarsDetailsViewModel = new AdminCarsDetailsViewModel(selectedCarStore, accountStore, websocketDataUpdateService);
        }
        public override void Dispose()
        {
            CarsListingViewModel.Dispose();
            AdminCarsDetailsViewModel.Dispose();

            base.Dispose();
        }
    }
}
