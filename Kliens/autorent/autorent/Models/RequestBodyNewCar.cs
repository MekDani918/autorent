using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Models
{
    public class RequestBodyNewCar
    {
        public string brand { get; set; }
        public string model { get; set; }
        public int categoryId { get; set; }
        public int dailyPrice { get; set; }
    }
}
