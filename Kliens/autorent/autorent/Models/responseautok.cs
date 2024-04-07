using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace autorent.Models
{
    internal class responseautok
    {
        public int id { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public int dailyPrice { get; set; }
        public List<DateTime> unavailableDates { get; set; }


    }
}
