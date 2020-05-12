using CRMService.Core.Domain.Entities.Enums;
using System;
using System.Collections.Generic;

namespace CRMService.Core.Domain.Entities
{
    public class CustomerAudit
    {
        public int CustomerAuditId { get; set; }
        public DateTime Date { get; set; }
        public CustomerAuditOperationType Operation { get; set; }      
        public virtual User User { get; set; }
        public virtual Customer Customer { get; set; }
    }
}