using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.DBContexts;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Data;
using Microsoft.Extensions.DependencyInjection;

namespace SlimeWeb.Core.Tools.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SlimeDbContext>
    {
        //public void ConfigureDesignTimeServices(IServiceCollection services)
        //{
        //    services.AddSingleton<IExportSqlService, ExportSqlService>();
        //}

    

        public SlimeDbContext CreateDbContext(string[] args)
        {
            // Load configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string connectionString;//= config.GetConnectionString("DefaultConnection");
            string provider = AppSettingsManager.GetDBEngine();
            connectionString = AppSettingsManager.GetDefaultConnectionString(provider);

            //config["DatabaseProvider"] ?? "mysql"; // Default to MySQL

            var optionsBuilder = new DbContextOptionsBuilder<SlimeDbContext>();

            // Configure the correct database provider dynamically
            switch (provider.ToLower())
            {
                case "mysql":
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    break;

                case "sqlserver":
                    optionsBuilder.UseSqlServer(connectionString);
                    break;

                //case "postgresql":
                //    optionsBuilder.UseNpgsql(connectionString);
                //    break;

                //case "sqlite":
                //    optionsBuilder.UseSqlite(connectionString);
                //    break;

                default:
                    throw new Exception($"Unsupported database provider: {provider}");
            }

            return new SlimeDbContext(optionsBuilder.Options);
        }
    }
}
