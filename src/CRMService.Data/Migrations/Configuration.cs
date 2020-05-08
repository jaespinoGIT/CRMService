using CRMService.Data.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace CRMService.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CRMService.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "CRMService.Data.DataContext";
        }

        protected override void Seed(DataContext ctx)
        {
            if (!ctx.Users.Any())
            {
                User user = new User()
                {                   
                    Name = "Admin1 Surname1",
                    Login = "AdminLogin",
                    Active = true,
                    UserRoles = new UserRole[]
                    {
                     new UserRole
                     {                        
                        Role = new Role
                        {                           
                            RoleName = "Admin"
                        }
                     }
                    }
                };

                ctx.Users.Add(user);

                if (!ctx.Customers.Any())
                {
                    Customer customer = new Customer()
                    {
                        Name = "Customer1",
                        Surname = "Surname1",
                        CustomerAudits = new CustomerAudit[]
                        {
                        new CustomerAudit
                        {
                            Date = DateTime.Now,
                            Operation = 0,
                            User = user
                        }

                        }

                    };

                    ctx.Customers.Add(customer);
                }

                base.Seed(ctx);
            }           

        }
    }
}
