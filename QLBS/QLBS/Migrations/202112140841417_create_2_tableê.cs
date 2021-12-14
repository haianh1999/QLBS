namespace QLBS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_2_tableê : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Saches",
                c => new
                    {
                        IDSach = c.String(nullable: false, maxLength: 128),
                        TenSach = c.String(),
                        GiaSach = c.String(),
                        MaTheLoai = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDSach)
                .ForeignKey("dbo.TheLoais", t => t.MaTheLoai, cascadeDelete: true)
                .Index(t => t.MaTheLoai);
            
            CreateTable(
                "dbo.TheLoais",
                c => new
                    {
                        MaTheLoai = c.Int(nullable: false, identity: true),
                        TenTheLoai = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaTheLoai);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Saches", "MaTheLoai", "dbo.TheLoais");
            DropIndex("dbo.Saches", new[] { "MaTheLoai" });
            DropTable("dbo.TheLoais");
            DropTable("dbo.Saches");
        }
    }
}
