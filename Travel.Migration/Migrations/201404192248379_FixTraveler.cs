namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTraveler : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Travelers", "TravelId", "dbo.Travels");
            DropIndex("dbo.Travelers", new[] { "TravelId" });
            CreateIndex("dbo.Travelers", "TravelId");
            AddForeignKey("dbo.Travelers", "TravelId", "dbo.Travels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Travelers", "TravelId", "dbo.Travels");
            DropIndex("dbo.Travelers", new[] { "TravelId" });
            CreateIndex("dbo.Travelers", "TravelId");
            AddForeignKey("dbo.Travelers", "TravelId", "dbo.Travels", "Id", cascadeDelete: true);
        }
    }
}
