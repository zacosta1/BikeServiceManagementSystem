using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using System.ComponentModel;
using BSMSData.Entities;
using BSMSSystem.DAL;
#endregion

namespace BSMSSystem.BLL
{
    [DataObject]
    public class CustomerController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Customer> List_Customers()
        {
            using (var context = new BSMSContext())
            {
                return context.Customers.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Customer> List_CustomersByLastName()
        {
            using (var context = new BSMSContext())
            {
                var results = from x in context.Customers
                              orderby x.LastName ascending
                              select x;
                return results.ToList();
            }
        }
    }
}
