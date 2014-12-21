namespace Maksimalist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailGrubu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailGrubus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailGrubus");
        }
    }
}
