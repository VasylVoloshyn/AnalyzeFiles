namespace AnalyzeFiles.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {            
            CreateTable(
                "dbo.AnalyzedColumnInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnalyzedFileInfoId = c.Int(nullable: false),
                        UniqueValues = c.Int(nullable: false),
                        MaxFoundItems = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AnalyzedFileInfoes", t => t.AnalyzedFileInfoId, cascadeDelete: true)
                .Index(t => t.AnalyzedFileInfoId);
            
            CreateTable(
                "dbo.AnalyzedFileInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rows = c.Int(nullable: false),
                        Name = c.String(),
                        IsFileCSV = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnalyzedColumnInfoes", "AnalyzedFileInfoId", "dbo.AnalyzedFileInfoes");
            DropIndex("dbo.AnalyzedColumnInfoes", new[] { "AnalyzedFileInfoId" });
            DropTable("dbo.AnalyzedFileInfoes");
            DropTable("dbo.AnalyzedColumnInfoes");
        }
    }
}
