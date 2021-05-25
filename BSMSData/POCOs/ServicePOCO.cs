﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSMSData.POCOs
{
    public class ServicePOCO
    {
        public int ServiceID { get; set; }
        public DateTime In { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Done { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public string VehicleIdentification { get; set; }
    }
}
