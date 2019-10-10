namespace Travel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFriendListItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserFriendListItems",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        FriendListItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.FriendListItem_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.FriendListItems", t => t.FriendListItem_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.FriendListItem_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserFriendListItems", "FriendListItem_Id", "dbo.FriendListItems");
            DropForeignKey("dbo.UserFriendListItems", "User_Id", "dbo.Users");
            DropIndex("dbo.UserFriendListItems", new[] { "FriendListItem_Id" });
            DropIndex("dbo.UserFriendListItems", new[] { "User_Id" });
            DropTable("dbo.UserFriendListItems");
        }
    }
}
