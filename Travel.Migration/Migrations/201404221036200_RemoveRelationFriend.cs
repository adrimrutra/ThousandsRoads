namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRelationFriend : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FriendsLists", "FriendList", "dbo.FriendListItems");
            DropForeignKey("dbo.FriendListItems", "UserId", "dbo.Users");
            DropForeignKey("dbo.Luggages", "UserId", "dbo.Users");
            DropIndex("dbo.FriendsLists", new[] { "FriendList" });
            DropIndex("dbo.FriendListItems", new[] { "UserId" });
            DropIndex("dbo.Luggages", new[] { "UserId" });
            DropColumn("dbo.FriendListItems", "UserId");
            DropColumn("dbo.FriendsLists", "FriendList");
            DropColumn("dbo.Luggages", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Luggages", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.FriendsLists", "FriendList", c => c.Int(nullable: false));
            AddColumn("dbo.FriendListItems", "UserId", c => c.Int());
            CreateIndex("dbo.Luggages", "UserId");
            CreateIndex("dbo.FriendListItems", "UserId");
            CreateIndex("dbo.FriendsLists", "FriendList");
            AddForeignKey("dbo.Luggages", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FriendListItems", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.FriendsLists", "FriendList", "dbo.FriendListItems", "Id");
        }
    }
}
