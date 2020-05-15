using CRMService.Core.Domain.Entities.Enums;
using System;

namespace CRMService.Models
{
    public class CustomerAuditModel
    {   
        
        public CustomerModel Customer { get; set; }       
        //We only need to show username and email
        public string UserUserName { get; set; }
        public string UserEmail { get; set; }

        public DateTime Date { get; set; }
        
        public CustomerAuditOperationType Operation { get; set; }

    }
}
