using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Models
{
    public class CustomerModel
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; }
        public byte[] Photo { get; set; }
    }
}
