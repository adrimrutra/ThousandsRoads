namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "UserId", c => c.Int());
            AddColumn("dbo.Messages", "MessengerId", c => c.Int());
            AddColumn("dbo.Messages", "TravelId", c => c.Int());
            CreateIndex("dbo.Messages", "MessengerId");
            CreateIndex("dbo.Messages", "TravelId");
            CreateIndex("dbo.Messages", "UserId");
            AddForeignKey("dbo.Messages", "MessengerId", "dbo.Users", "Id");
            AddForeignKey("dbo.Messages", "TravelId", "dbo.Travels", "Id");
            AddForeignKey("dbo.Messages", "UserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "TravelId", "dbo.Travels");
            DropForeignKey("dbo.Messages", "MessengerId", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "TravelId" });
            DropIndex("dbo.Messages", new[] { "MessengerId" });
            DropColumn("dbo.Messages", "TravelId");
            DropColumn("dbo.Messages", "MessengerId");
            DropColumn("dbo.Messages", "UserId");
        }
    }
}
