using autorent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.ViewModels
{
    public class CarsListingItemViewModel : ViewModelBase
    {
        public Car Car { get; }

        public string Brand => Car.Brand;
        public string Model => Car.Model;
        public string Category => Car.Category;
        public int Price => Car.DailyPrice;

        public CarsListingItemViewModel(Car car)
        {
            Car = car;
        }
    }
}
