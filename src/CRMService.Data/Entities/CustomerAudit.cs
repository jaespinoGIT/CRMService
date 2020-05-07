using System;
using System.Collections.Generic;

namespace CRMService.Data.Entities
{
    public class CustomerAudit
    {
        public int CustomerAuditId { get; set; }
        public DateTime Date { get; set; }
        public short Operation { get; set; }      
        public User User { get; set; }
        public Customer Customer { get; set; }
    }
}