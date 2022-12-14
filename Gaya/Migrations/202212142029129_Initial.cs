namespace Gaya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstField = c.String(nullable: false),
                        SecondField = c.String(nullable: false),
                        ProcessorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Processors", t => t.ProcessorId, cascadeDelete: true)
                .Index(t => t.ProcessorId);
            
            CreateTable(
                "dbo.Processors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Action = c.String(nullable: false),
                        FirstParameterAsString = c.Boolean(nullable: false),
                        SecondParameterAsString = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Histories", "ProcessorId", "dbo.Processors");
            DropIndex("dbo.Histories", new[] { "ProcessorId" });
            DropTable("dbo.Processors");
            DropTable("dbo.Histories");
        }
    }
}
