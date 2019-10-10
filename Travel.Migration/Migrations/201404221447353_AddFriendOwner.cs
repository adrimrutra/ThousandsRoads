namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFriendOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FriendListItems", "OwnerId", c => c.Int());
            CreateIndex("dbo.FriendListItems", "OwnerId");
            AddForeignKey("dbo.FriendListItems", "OwnerId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FriendListItems", "OwnerId", "dbo.Users");
            DropIndex("dbo.FriendListItems", new[] { "OwnerId" });
            DropColumn("dbo.FriendListItems", "OwnerId");
        }
    }
}
