using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using BSMSData.Entities;
using BSMSSystem.DAL;
#endregion

namespace BSMSSystem.BLL
{
    public class EmployeeController
    {
        public Employee Employee_Get(int employeeid)
        {
            using (var context = new BSMSContext())
            {
                return context.Employees.Find(employeeid);
            }
        }
    }
}
