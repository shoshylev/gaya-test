namespace Gaya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Histories", "Result", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Histories", "Result");
        }
    }
}
