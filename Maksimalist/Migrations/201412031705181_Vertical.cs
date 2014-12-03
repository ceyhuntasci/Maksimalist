namespace Maksimalist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vertical : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Vertical", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Vertical");
        }
    }
}
