namespace AnalyzeFiles.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_row : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnalyzedFileColumnHistoryInfoes", "Row", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnalyzedFileColumnHistoryInfoes", "Row");
        }
    }
}
