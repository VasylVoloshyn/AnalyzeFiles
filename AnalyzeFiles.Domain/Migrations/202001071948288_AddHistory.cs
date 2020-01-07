namespace AnalyzeFiles.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnalyzedFileHistoryInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        AnalyzedFileInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AnalyzedFileInfoes", t => t.AnalyzedFileInfoId, cascadeDelete: true)
                .Index(t => t.AnalyzedFileInfoId);
            
            CreateTable(
                "dbo.AnalyzedFileColumnHistoryInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnalyzedFileHistoryInfoId = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AnalyzedFileHistoryInfoes", t => t.AnalyzedFileHistoryInfoId, cascadeDelete: true)
                .Index(t => t.AnalyzedFileHistoryInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AnalyzedFileColumnHistoryInfoes", "AnalyzedFileHistoryInfoId", "dbo.AnalyzedFileHistoryInfoes");
            DropForeignKey("dbo.AnalyzedFileHistoryInfoes", "AnalyzedFileInfoId", "dbo.AnalyzedFileInfoes");
            DropIndex("dbo.AnalyzedFileColumnHistoryInfoes", new[] { "AnalyzedFileHistoryInfoId" });
            DropIndex("dbo.AnalyzedFileHistoryInfoes", new[] { "AnalyzedFileInfoId" });
            DropTable("dbo.AnalyzedFileColumnHistoryInfoes");
            DropTable("dbo.AnalyzedFileHistoryInfoes");
        }
    }
}
