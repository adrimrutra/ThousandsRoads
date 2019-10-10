namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Friendlist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FriendsListUsers",
                c => new
                    {
                        FriendsList_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FriendsList_Id, t.User_Id })
                .ForeignKey("dbo.FriendsLists", t => t.FriendsList_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.FriendsList_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.FriendsLists", "FriendList", c => c.Int(nullable: false));
            CreateIndex("dbo.FriendsLists", "FriendList");
            AddForeignKey("dbo.FriendsLists", "FriendList", "dbo.FriendListItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FriendsLists", "FriendList", "dbo.FriendListItems");
            DropForeignKey("dbo.FriendsListUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.FriendsListUsers", "FriendsList_Id", "dbo.FriendsLists");
            DropIndex("dbo.FriendsLists", new[] { "FriendList" });
            DropIndex("dbo.FriendsListUsers", new[] { "User_Id" });
            DropIndex("dbo.FriendsListUsers", new[] { "FriendsList_Id" });
            DropColumn("dbo.FriendsLists", "FriendList");
            DropTable("dbo.FriendsListUsers");
        }
    }
}
