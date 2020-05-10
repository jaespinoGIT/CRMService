using CRMService.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Models
{
    public class CustomerAuditModel
    {   
        [Required]
        public CustomerModel Customer { get; set; }
        [Required]
        public UserModel User { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public CustomerAuditOperationType Operation { get; set; }

    }
}
