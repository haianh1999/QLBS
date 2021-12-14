namespace QLBS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TheLoais", "TenTheLoai", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TheLoais", "TenTheLoai", c => c.Int(nullable: false));
        }
    }
}
