namespace Miubuy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _020201 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChatType = c.Int(nullable: false),
                        SenderId = c.Int(nullable: false),
                        RecipientId = c.Int(nullable: false),
                        RoomId = c.Int(nullable: false),
                        Message = c.String(),
                        MsgTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountyId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Delete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        CountyId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                        SellerId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Picture = c.String(),
                        Rule = c.String(),
                        TagText = c.String(),
                        MaxUsers = c.Int(nullable: false),
                        Star = c.Int(nullable: false),
                        R18 = c.Boolean(nullable: false),
                        RoomClose = c.Boolean(nullable: false),
                        RoomStart = c.DateTime(nullable: false),
                        RoomEnd = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.Counties", t => t.CountyId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.CountryId)
                .Index(t => t.CountyId)
                .Index(t => t.CityId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Delete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Counties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Delete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SellerId = c.Int(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        RoomId = c.Int(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Payment = c.Int(nullable: false),
                        Pickup = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        BuyerStar = c.Int(nullable: false),
                        BuyerReviews = c.String(),
                        SellerStar = c.Int(nullable: false),
                        SellerReviews = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.SellerId, cascadeDelete: true)
                .Index(t => t.SellerId)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConnectionId = c.String(),
                        Account = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        PasswordSalt = c.String(),
                        Nickname = c.String(),
                        Picture = c.String(),
                        Name = c.String(),
                        Birthday = c.DateTime(),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        BuyerAverageStar = c.Single(nullable: false),
                        SellerAverageStar = c.Single(nullable: false),
                        Permission = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoomUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoomId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoomId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Color = c.String(),
                        Delete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TempDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BuyerId = c.Int(nullable: false),
                        RoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rooms", "TagId", "dbo.Tags");
            DropForeignKey("dbo.RoomUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.RoomUsers", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.Orders", "SellerId", "dbo.Users");
            DropForeignKey("dbo.Orders", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Rooms", "CountyId", "dbo.Counties");
            DropForeignKey("dbo.Rooms", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Rooms", "CityId", "dbo.Cities");
            DropIndex("dbo.RoomUsers", new[] { "UserId" });
            DropIndex("dbo.RoomUsers", new[] { "RoomId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "RoomId" });
            DropIndex("dbo.Orders", new[] { "SellerId" });
            DropIndex("dbo.Rooms", new[] { "TagId" });
            DropIndex("dbo.Rooms", new[] { "CityId" });
            DropIndex("dbo.Rooms", new[] { "CountyId" });
            DropIndex("dbo.Rooms", new[] { "CountryId" });
            DropTable("dbo.TempDetails");
            DropTable("dbo.Tags");
            DropTable("dbo.RoomUsers");
            DropTable("dbo.Users");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
            DropTable("dbo.Counties");
            DropTable("dbo.Countries");
            DropTable("dbo.Rooms");
            DropTable("dbo.Cities");
            DropTable("dbo.ChatHistories");
        }
    }
}
