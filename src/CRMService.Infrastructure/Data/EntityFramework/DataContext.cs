using CRMService.Core.Domain.Entities;
using CRMService.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Infrastructure.Data.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext() : base("CRMServiceConnectionString")
        {
#if DEBUG
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
#endif
        }

        public DbSet<Customer> Customers { get; set; }     

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<CustomerAudit> CustomerAudits { get; set; }

    }
}
