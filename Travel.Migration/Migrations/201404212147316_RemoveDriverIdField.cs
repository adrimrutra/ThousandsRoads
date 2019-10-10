namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDriverIdField : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Travels", "DriverId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Travels", "DriverId", c => c.Int(nullable: false));
        }
    }
}
