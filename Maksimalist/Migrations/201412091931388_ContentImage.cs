namespace Maksimalist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "ContentImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "ContentImage");
        }
    }
}
