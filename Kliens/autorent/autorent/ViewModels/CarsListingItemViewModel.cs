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

        public string Brand
        {
            get => Car.Brand;
            set
            {
                Car.Brand = value;
                OnPropertyChanged(nameof(Brand));
            }
        }
        public string Model
        {
            get => Car.Model;
            set
            {
                Car.Model = value;
                OnPropertyChanged(nameof(Model));
            }
        }
        public string Category
        {
            get => Car.Category;
            set
            {
                Car.Category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        public int Price
        {
            get => Car.DailyPrice;
            set
            {
                Car.DailyPrice = value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(DiscountedPrice));
            }
        }
        public double DiscountPercentage
        {
            get => Car.DiscountPercentage;
            set
            {
                Car.DiscountPercentage = value;
                OnPropertyChanged(nameof(DiscountPercentage));
                OnPropertyChanged(nameof(DiscountedPrice));
                OnPropertyChanged(nameof(IsDiscounted));
            }
        }
        public int DiscountedPrice => (int)Math.Round((100 - DiscountPercentage) / 100 * Car.DailyPrice);
        public bool IsDiscounted => DiscountPercentage > 0;

        public CarsListingItemViewModel(Car car)
        {
            Car = car;
        }
    }
}
