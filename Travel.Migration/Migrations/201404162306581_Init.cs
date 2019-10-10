namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(),
                        Avatar = c.String(),
                        Email = c.String(),
                        Rating = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        MessengerId = c.Int(nullable: false),
                        Message = c.String(),
                        Type = c.Int(nullable: false),
                        Data = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.MessengerId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.MessengerId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FriendListItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FriendsLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FriendList = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FriendListItems", t => t.FriendList)
                .Index(t => t.FriendList);
            
            CreateTable(
                "dbo.Luggages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TravelId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Startpoint = c.String(),
                        Endpoint = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Travels", t => t.TravelId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TravelId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Travels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DriverId = c.Int(nullable: false),
                        Capacity = c.Int(),
                        Startdate = c.DateTime(nullable: false),
                        Enddate = c.DateTime(nullable: false),
                        DisplayName = c.String(),
                        Mapsnapshot = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MapPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        TravelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Travels", t => t.TravelId, cascadeDelete: true)
                .Index(t => t.TravelId);
            
            CreateTable(
                "dbo.Travelers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TravelId = c.Int(nullable: false),
                        Usertype = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Travels", t => t.TravelId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TravelId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tokentype = c.Int(nullable: false),
                        SocialId = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Theme = c.String(),
                        MessageText = c.String(),
                        State = c.Boolean(nullable: false),
                        Direction = c.Int(nullable: false),
                        Luggage = c.Int(nullable: false),
                        PersonCount = c.Int(nullable: false),
                        MessengerId = c.Int(nullable: false),
                        TravelId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.MessengerId)
                .ForeignKey("dbo.Travels", t => t.TravelId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.MessengerId)
                .Index(t => t.TravelId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.Messages", "MessengerId", "dbo.Users");
            DropForeignKey("dbo.Tokens", "UserId", "dbo.Users");
            DropForeignKey("dbo.Luggages", "UserId", "dbo.Users");
            DropForeignKey("dbo.Luggages", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.Travelers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Travelers", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.MapPoints", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.FriendListItems", "UserId", "dbo.Users");
            DropForeignKey("dbo.FriendsLists", "FriendList", "dbo.FriendListItems");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "MessengerId", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "TravelId" });
            DropIndex("dbo.Messages", new[] { "MessengerId" });
            DropIndex("dbo.Tokens", new[] { "UserId" });
            DropIndex("dbo.Luggages", new[] { "UserId" });
            DropIndex("dbo.Luggages", new[] { "TravelId" });
            DropIndex("dbo.Travelers", new[] { "UserId" });
            DropIndex("dbo.Travelers", new[] { "TravelId" });
            DropIndex("dbo.MapPoints", new[] { "TravelId" });
            DropIndex("dbo.FriendListItems", new[] { "UserId" });
            DropIndex("dbo.FriendsLists", new[] { "FriendList" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "MessengerId" });
            DropTable("dbo.Messages");
            DropTable("dbo.Tokens");
            DropTable("dbo.Travelers");
            DropTable("dbo.MapPoints");
            DropTable("dbo.Travels");
            DropTable("dbo.Luggages");
            DropTable("dbo.FriendsLists");
            DropTable("dbo.FriendListItems");
            DropTable("dbo.Comments");
            DropTable("dbo.Users");
        }
    }
}
