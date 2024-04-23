using autorent.Models;
using autorent.Stores;
using autorent.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Commands
{
    public class AdminCarsCreateCommand : CommandBase
    {
        private readonly SelectedCarStore _selectedCarStore;
        public AdminCarsCreateCommand(SelectedCarStore selectedCarStore)
        {
            _selectedCarStore = selectedCarStore;
        }

        public override void Execute(object? parameter)
        {
            _selectedCarStore.SelectedCar = new Car(-1, "", "", "", 0, new List<string>());
        }
    }
}
