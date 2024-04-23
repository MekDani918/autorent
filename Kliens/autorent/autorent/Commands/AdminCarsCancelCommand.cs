using autorent.Stores;
using autorent.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Commands
{
    public class AdminCarsCancelCommand : CommandBase
    {
        private readonly AdminCarsDetailsViewModel _viewModel;
        private readonly SelectedCarStore _selectedCarStore;
        public AdminCarsCancelCommand(AdminCarsDetailsViewModel viewModel, SelectedCarStore selectedCarStore)
        {
            _viewModel = viewModel;
            _selectedCarStore = selectedCarStore;
        }

        public override void Execute(object? parameter)
        {
            if (_viewModel.IsEditEnabled && _viewModel.SelectedCar.Id != -1)
            {
                _viewModel.IsEditEnabled = !_viewModel.IsEditEnabled;
                _viewModel.OnSelectedCarChanged();
            }
            else
            {
                _selectedCarStore.SelectedCar = null;
            }
        }
    }
}
