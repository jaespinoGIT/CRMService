using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Models
{
    public class UserRoleModel
    {     
        [Required]
        public RoleModel Role { get; set; }
        [Required]
        public UserModel User { get; set; }
    }
}
