using autorent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Stores
{
    public class SelectedCarStore
    {
        private Car _selectedCar;
        public Car SelectedCar
        {
            get => _selectedCar;
            set
            {
                _selectedCar = value;
                SelectedCarChanged?.Invoke();
            }
        }

        public event Action SelectedCarChanged;
    }
}
