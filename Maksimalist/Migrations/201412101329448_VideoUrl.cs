namespace Maksimalist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "VideoUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "VideoUrl");
        }
    }
}
