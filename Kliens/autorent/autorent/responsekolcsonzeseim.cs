﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autorent
{
    internal class responsekolcsonzeseim
    {
        public int carId {  get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string rentalTimestamp { get; set; }
    }
}