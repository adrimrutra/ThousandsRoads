namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTravelTravelers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Travelers", "TravelId", "dbo.Travels");
            DropIndex("dbo.Travelers", new[] { "TravelId" });
            DropColumn("dbo.Travelers", "TravelId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Travelers", "TravelId", c => c.Int(nullable: false));
            CreateIndex("dbo.Travelers", "TravelId");
            AddForeignKey("dbo.Travelers", "TravelId", "dbo.Travels", "Id");
        }
    }
}
