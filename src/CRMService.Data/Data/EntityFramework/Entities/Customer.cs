using System;
using System.Collections.Generic;

namespace CRMService.Infrastructure.Data.EntityFramework.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte[] Photo { get; set; }

        public ICollection<CustomerAudit> CustomerAudits { get; set; }
    }
}