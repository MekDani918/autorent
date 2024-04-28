using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public Car Car { get; set; }
        public string Description {  get; set; }
        public int Percent {  get; set; }
    }
}
