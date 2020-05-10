namespace CRMService.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerAudits",
                c => new
                    {
                        CustomerAuditId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Operation = c.Short(nullable: false),
                        Customer_CustomerId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CustomerAuditId)
                .ForeignKey("dbo.Customers", t => t.Customer_CustomerId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.Customer_CustomerId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Photo = c.Binary(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Login = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserRoleId = c.Int(nullable: false, identity: true),
                        Role_RoleId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.UserRoleId)
                .ForeignKey("dbo.Roles", t => t.Role_RoleId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.Role_RoleId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        RoleIsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerAudits", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "Role_RoleId", "dbo.Roles");
            DropForeignKey("dbo.CustomerAudits", "Customer_CustomerId", "dbo.Customers");
            DropIndex("dbo.UserRoles", new[] { "User_UserId" });
            DropIndex("dbo.UserRoles", new[] { "Role_RoleId" });
            DropIndex("dbo.CustomerAudits", new[] { "User_UserId" });
            DropIndex("dbo.CustomerAudits", new[] { "Customer_CustomerId" });
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Users");
            DropTable("dbo.Customers");
            DropTable("dbo.CustomerAudits");
        }
    }
}
