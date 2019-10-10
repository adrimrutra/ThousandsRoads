namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTravelTravelers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Travelers", "TravelId", c => c.Int());
            CreateIndex("dbo.Travelers", "TravelId");
            AddForeignKey("dbo.Travelers", "TravelId", "dbo.Travels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Travelers", "TravelId", "dbo.Travels");
            DropIndex("dbo.Travelers", new[] { "TravelId" });
            DropColumn("dbo.Travelers", "TravelId");
        }
    }
}
