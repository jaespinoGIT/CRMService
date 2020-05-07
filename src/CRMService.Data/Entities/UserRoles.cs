using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Data.Entities
{
    public class UserRoles
    {
        public int UserRolesId { get; set; }
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
