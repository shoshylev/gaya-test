namespace Gaya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Histories", "ExecutionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Histories", "ExecutionDate");
        }
    }
}
