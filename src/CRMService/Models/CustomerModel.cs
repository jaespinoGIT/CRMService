using CRMService.Helpers.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRMService.Models
{
    public class CustomerModel
    {       
        public int CustomerId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; }        
        public byte[] Photo { get; set; }

        public ICollection<CustomerAuditModel> CustomerAudits { get; set; }       
    }
}
