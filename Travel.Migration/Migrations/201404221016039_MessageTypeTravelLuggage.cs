namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageTypeTravelLuggage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Luggages", "TravelId", "dbo.Travels");
            DropIndex("dbo.Luggages", new[] { "TravelId" });
            AddColumn("dbo.Travels", "Luggage", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Luggages", "TravelId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Luggages", "TravelId", c => c.Int(nullable: false));
            DropColumn("dbo.Messages", "Type");
            DropColumn("dbo.Travels", "Luggage");
            CreateIndex("dbo.Luggages", "TravelId");
            AddForeignKey("dbo.Luggages", "TravelId", "dbo.Travels", "Id", cascadeDelete: true);
        }
    }
}
