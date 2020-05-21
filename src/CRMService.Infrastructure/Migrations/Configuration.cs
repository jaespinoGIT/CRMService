using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using CRMService.Infrastructure.Data.EntityFramework;
using CRMService.Infrastructure.Data;
using CRMService.Core.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CRMService.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CRMService.Infrastructure.DataContext";
        }
        /// <summary>
        /// Fill the database with the initial data
        /// </summary>
        /// <param name="ctx"></param>
        protected override void Seed(DataContext ctx)
        {
            var manager = new UserManager<User>(new UserStore<User>(ctx));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));

            var user = new User()
            {
                UserName = "SuperPowerUser",
                Email = "examplemail@gmail.com",
                EmailConfirmed = true,
                FirstName = "User",
                LastName = "LastName"
            };

            manager.Create(user, "b7Wpvk$t4vvyBG");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByName("SuperPowerUser");

            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });

            base.Seed(ctx);


        }
    }
}
