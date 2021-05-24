﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace BSMSData.Entities.Security
{
    // You can add User data for the user by adding more properties to your User class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //we can add addition attributes that will be physically
        //include into the AspNetUsers security
        public int? EmployeeID { get; set; }
        public int? CustomerID { get; set; }
    }
}
