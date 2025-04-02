using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using Microsoft.EntityFrameworkCore.Migrations;
using SlimeWeb.Core.Data.MySQL;
using System;
using System.IO;
using System.Linq;

namespace SlimeWeb.Core.Data.DBContexts
{    //
    public class SlimeDbContext : IdentityDbContext<ApplicationUser, 
        ApplicationRole, string>, IStorageContext
    {
       
       
       
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Category> Catgories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Files> Files { get; set; }
       
        public DbSet<Feature> Features { get; set; }
        public DbSet<GeneralSettings> Settings { get; set; }
        public GeneralSettings GeneralSettings
        {
            get
            {
                return this.Settings.First();
            }
        }
        public DbSet<BannedUsers> BannedUsers { get; set; }
        public DbSet<News> News{ get; set; }
        public DbSet<BlogMods>  BlogMods { get; set; }

        public DbSet<CategotyPost> CategoryPosts  { get; set; }
        public DbSet<TagPost> TagPosts { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<FilesPostBlog> FilesPostsBlog { get; set; }
        public DbSet<CategotyNews> CategoryNews { get; set; }
        public DbSet<TagNews> TagNews { get; set; }
        public DbSet< SlimeWebPage> Pages { get; set; }
        public DbSet<FilesPages> FilesPages { get; set; }

        //public IStorageContext StorageContext => throw new NotImplementedException();\

        public SlimeDbContext(DbContextOptions<SlimeDbContext> options)
               : base(options)
        {

            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public SlimeDbContext()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            try
            {

                //string pathwithextention= System.Reflection.Assembly.GetExecutingAssembly().CodeBase;


                //   string path = System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\","");


                optionsBuilder.EnableSensitiveDataLogging (true);
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
               

                var directory = FileSystemManager.GetAppRootDataFolderAbsolutePath();
                if (Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }

                string olddbConn = AppSettingsManager.GetDefaultConnectionString(AppSettingsManager.
                    GetDBEngine().ToString());
                if (olddbConn != null)
                {

                    string dbCon = olddbConn.Replace("|DataDirectory|", directory);
                    if (dbCon != null)
                    {
                        if (AppSettingsManager.GetDBEngine() == enumDBEngine.SQLServer.ToString())
                        {
                            optionsBuilder.UseSqlServer(dbCon, x => x.MigrationsAssembly
                            ("SlimeWeb.Core.Migrations.SQLServerMigrations")).
                                UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); 
                        }
                        else if (AppSettingsManager.GetDBEngine() == enumDBEngine.MySQl.ToString())
                        {
                            optionsBuilder.UseMySql(dbCon,
                                 ServerVersion.AutoDetect(dbCon),
                                x => x.MigrationsAssembly("SlimeWeb.Core.Migrations.MySQLMigrations")).
                                UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                 .ReplaceService<IHistoryRepository, CustomMySqlHistoryRepository>(); // Use your custom repository;


                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                 
            }



        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            try
            {
                base.OnModelCreating(builder);
                builder.Entity<ApplicationUser>()
                     .Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                builder.Entity<SlimeWebPage>().HasIndex(e => e.Name).IsUnique();
                builder.Entity<GeneralSettings>().Property(e => e.ItemsPerPage)
                    .HasDefaultValue(10);
                    
                    
                    
             }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                 
            }


        }
        public override int SaveChanges()
        {
            
            foreach (var entry in this.ChangeTracker.Entries<Blog>().ToList())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.LastUpdate = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastUpdate = DateTime.Now;

                }
            }
            return base.SaveChanges();
        }


    }
}
