namespace Gaya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial7 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Histories", "ExecutionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Histories", "ExecutionDate", c => c.DateTime());
        }
    }
}
