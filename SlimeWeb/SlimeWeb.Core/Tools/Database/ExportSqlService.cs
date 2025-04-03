using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SlimeWeb.Core.Data.DBContexts;
using System;
using System.IO;

namespace SlimeWeb.Core.Tools.Database
{
    public class ExportSqlService //: IExportSqlService
    {
        private static    SlimeDbContext _dbContext;

        public ExportSqlService(SlimeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static void ExportSql(string outputPath)
        {
            var migrator = _dbContext.Database.GetService<IMigrator>();
            string sqlScript = migrator.GenerateScript(fromMigration: null, toMigration: null);
            File.WriteAllText(outputPath, sqlScript);
            Console.WriteLine($"SQL script exported to {outputPath}");
        }
    }
}
