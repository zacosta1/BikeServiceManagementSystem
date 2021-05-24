using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using Microsoft.AspNet.Identity.EntityFramework;
using BSMSData.Entities.Security;
#endregion

namespace BSMSSystem.DAL.Security
{
    internal class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}
