using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Models
{
    public class RequestBodyNewSale
    {
        public int carId {  get; set; }
        public string description { get; set; }
        public int percent {  get; set; }
    }
}
