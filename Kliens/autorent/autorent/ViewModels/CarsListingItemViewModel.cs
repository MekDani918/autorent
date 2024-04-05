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
        public double DiscountPercentage => Car.DiscountPercentage;
        public int DiscountedPrice => (int)Math.Round((100 - DiscountPercentage) / 100 * Car.DailyPrice);
        public bool IsDiscounted => DiscountPercentage > 0;

        public CarsListingItemViewModel(Car car)
        {
            Car = car;
        }
    }
}
