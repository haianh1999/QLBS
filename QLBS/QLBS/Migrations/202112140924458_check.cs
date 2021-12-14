namespace QLBS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TacGias",
                c => new
                    {
                        MaTacGia = c.Int(nullable: false, identity: true),
                        TenTacGia = c.String(),
                    })
                .PrimaryKey(t => t.MaTacGia);
            
            AddColumn("dbo.Saches", "MaTacGia", c => c.Int(nullable: false));
            CreateIndex("dbo.Saches", "MaTacGia");
            AddForeignKey("dbo.Saches", "MaTacGia", "dbo.TacGias", "MaTacGia", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Saches", "MaTacGia", "dbo.TacGias");
            DropIndex("dbo.Saches", new[] { "MaTacGia" });
            DropColumn("dbo.Saches", "MaTacGia");
            DropTable("dbo.TacGias");
        }
    }
}
