using CRMService.Core.Domain.Entities;
using CRMService.Data.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Infrastructure.Data.EntityFramework
{
    public class DataContext : IdentityDbContext<User>
    {

        public static DataContext Create()
        {
            return new DataContext();
        }
        public DataContext() : base("CRMServiceConnectionString", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
#if DEBUG
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
#endif
        }

        public DbSet<Customer> Customers { get; set; }          

        public DbSet<CustomerAudit> CustomerAudits { get; set; }

    }
}
