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
            ContextKey = "CRMService.Data.CampContext";
        }

        protected override void Seed(DataContext ctx)
        {
         
        }
    }
}
