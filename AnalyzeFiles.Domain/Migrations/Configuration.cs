namespace AnalyzeFiles.Domain.Migrations
{
    using AnalyzeFiles.Domain.DataBase;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AnalyzedFilesDBContext>
    {
        public Configuration()
        {             
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AnalyzedFilesDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
