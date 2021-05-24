using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSMSData.Entities.Security
{
    public static class SecurityRoles
    {
        public const string WebsiteAdmins = "Website Admins";
        public const string RegisteredUsers = "Registered Users";
        public const string Staff = "Staff";
        public const string WarehouseStaff = "Clerk";
        public const string SalesStaff = "Salesperson";
        public const string ServicingStaff = "Mechanic";
        public static List<string> DefaultSecurityRoles
        {
            get
            {
                List<string> value = new List<string>
                {
                    WebsiteAdmins,
                    RegisteredUsers,
                    Staff,
                    WarehouseStaff,
                    SalesStaff,
                    ServicingStaff
                };
                return value;
            }
        }
    }
}
