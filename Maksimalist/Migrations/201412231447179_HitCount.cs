namespace Maksimalist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HitCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "HitCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "HitCount");
        }
    }
}
