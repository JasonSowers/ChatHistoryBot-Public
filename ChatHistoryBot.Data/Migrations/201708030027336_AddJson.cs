namespace ChatHistoryBot.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "Json", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "Json");
        }
    }
}
