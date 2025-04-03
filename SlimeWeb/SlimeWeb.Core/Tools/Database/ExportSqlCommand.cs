using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System.Management.Automation;
using Microsoft.AspNetCore.Components;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Managers;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SlimeWeb.Core.Tools.Database
{
    [Cmdlet(VerbsData.Export, "Sql")]
    public class ExportSqlCommand : Cmdlet
    {
        [System.Management.Automation.Parameter(Position = 0,
            HelpMessage = "The path to save the SQL script to.")]
        public string OutputPath { get; set; } = "migrations.sql";

        protected override void ProcessRecord()
        {
            ExportSql(OutputPath);
        }

        public static void ExportSql(string outputPath = "migrations.sql")
        {
            var services = new ServiceCollection();
            services.AddDbContext<SlimeDbContext>(options =>
            {
                // Load your configuration
                string provider = AppSettingsManager.GetDBEngine();
                string connectionString = AppSettingsManager.GetDefaultConnectionString(provider);

                switch (provider.ToLower())
                {
                    case "mysql":
                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                        break;
                    case "sqlserver":
                        options.UseSqlServer(connectionString);
                        break;
                    //case "postgresql":
                    //    options.UseNpgsql(connectionString);
                    //    break;
                    //case "sqlite":
                    //    options.UseSqlite(connectionString);
                    //    break;
                    default:
                        throw new Exception($"Unsupported database provider: {provider}");
                }
            });

            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetRequiredService<SlimeDbContext>();
            var migrator = dbContext.Database.GetService<IMigrator>();

            string sqlScript = migrator.GenerateScript(fromMigration: null, toMigration: null);

            File.WriteAllText(outputPath, sqlScript);
            Console.WriteLine($"SQL script exported to {outputPath}");
        }
    }
}
