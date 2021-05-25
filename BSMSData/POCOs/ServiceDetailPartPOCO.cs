using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSMSData.POCOs
{
    public class ServiceDetailPartPOCO
    {
        public int ServiceDetailID { get; set; }
        public int ServiceDetailPartID { get; set; }
        public int PartID { get; set; }
        public string PartDescription { get; set; }
        public int Quantity { get; set; }
    }
}
