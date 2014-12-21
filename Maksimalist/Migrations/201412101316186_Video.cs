namespace Maksimalist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Video : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "HasVideo", c => c.Boolean(nullable: false));
            DropColumn("dbo.Posts", "Vertical");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "Vertical", c => c.Boolean(nullable: false));
            DropColumn("dbo.Posts", "HasVideo");
        }
    }
}
