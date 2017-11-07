namespace ChatHistoryBot.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userKeyTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserKeys",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        Key = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserKeys");
        }
    }
}
