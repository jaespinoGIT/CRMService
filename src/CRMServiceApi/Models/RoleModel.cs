using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Models
{
    public class RoleModel
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string RoleName { get; set; }
    }
}
