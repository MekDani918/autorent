using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int DailyPrice { get; set; }
        public List<string> UnavailableDates { get; set; }

        public Car(int id, string category, string brand, string model, int dailyPrice, List<string> unavailableDates)
        {
            Id = id;
            Category = category;
            Brand = brand;
            Model = model;
            DailyPrice = dailyPrice;
            UnavailableDates = unavailableDates;
        }
    }
}
