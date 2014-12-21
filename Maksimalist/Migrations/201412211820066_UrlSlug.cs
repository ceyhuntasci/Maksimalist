namespace Maksimalist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UrlSlug : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "UrlSlug", c => c.String());
            AddColumn("dbo.SubCategories", "UrlSlug", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubCategories", "UrlSlug");
            DropColumn("dbo.Categories", "UrlSlug");
        }
    }
}
