using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using BSMSData.POCOs;
#endregion

namespace BSMSData.DTOs
{
    public class ServiceDetailDTO
    {
        public int ServiceDetailID { get; set; }
        public int ServiceID { get; set; }
        public string Description { get; set; }
        public decimal ServiceDetailHours { get; set; }
        public string Coupon { get; set; }
        public string Comments { get; set; }
        //public bool? Status { get; set; }
        public string Status { get; set; }
        public IEnumerable<ServiceDetailPartPOCO> ServiceDetailParts { get; set; }
    }
}
