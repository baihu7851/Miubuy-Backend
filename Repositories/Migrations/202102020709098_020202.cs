namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _020202 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "SellerId", "dbo.Users");
            AddColumn("dbo.Orders", "User_Id", c => c.Int());
            CreateIndex("dbo.Orders", "BuyerId");
            CreateIndex("dbo.Orders", "User_Id");
            AddForeignKey("dbo.Orders", "BuyerId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "User_Id", "dbo.Users", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Orders", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Orders", "BuyerId", "dbo.Users");
            DropIndex("dbo.Orders", new[] { "User_Id" });
            DropIndex("dbo.Orders", new[] { "BuyerId" });
            DropColumn("dbo.Orders", "User_Id");
            AddForeignKey("dbo.Orders", "SellerId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}