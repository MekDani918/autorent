using autorent.Stores;
using autorent.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Commands
{
    public class AdminCarsEditCommand : CommandBase
    {
        private readonly AdminCarsDetailsViewModel _viewModel;
        public AdminCarsEditCommand(AdminCarsDetailsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.IsEditEnabled = !_viewModel.IsEditEnabled;
        }
    }
}
