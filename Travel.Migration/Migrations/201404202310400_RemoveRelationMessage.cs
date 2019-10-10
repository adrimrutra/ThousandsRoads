namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRelationMessage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "MessengerId", "dbo.Users");
            DropForeignKey("dbo.Messages", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.Messages", "UserId", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "MessengerId" });
            DropIndex("dbo.Messages", new[] { "TravelId" });
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropColumn("dbo.Messages", "MessengerId");
            DropColumn("dbo.Messages", "TravelId");
            DropColumn("dbo.Messages", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "TravelId", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "MessengerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Messages", "UserId");
            CreateIndex("dbo.Messages", "TravelId");
            CreateIndex("dbo.Messages", "MessengerId");
            AddForeignKey("dbo.Messages", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Messages", "TravelId", "dbo.Travels", "Id");
            AddForeignKey("dbo.Messages", "MessengerId", "dbo.Users", "Id");
        }
    }
}
