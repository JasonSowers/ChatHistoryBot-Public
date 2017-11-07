namespace ChatHistoryBot.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserConversation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserConversations",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        ConversationId = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserConversations");
        }
    }
}
