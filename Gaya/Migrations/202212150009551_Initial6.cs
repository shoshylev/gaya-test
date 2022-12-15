namespace Gaya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Histories", "ExecutionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Histories", "ExecutionDate", c => c.DateTime(nullable: false));
        }
    }
}
