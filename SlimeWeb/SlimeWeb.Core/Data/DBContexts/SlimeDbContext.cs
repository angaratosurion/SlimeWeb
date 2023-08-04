using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.DBContexts
{    //
    public class SlimeDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IStorageContext
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
        public DbSet<FilesPostBlog> FilesPostsBlog { get; set; }
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




                var directory = FileSystemManager.GetAppRootDataFolderAbsolutePath();
                if (Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }
                string olddbConn = AppSettingsManager.GetDefaultConnectionString();
                if (olddbConn != null)
                {

                    string dbCon = olddbConn.Replace("|DataDirectory|", directory);
                    if (dbCon != null)
                    {
                        if (AppSettingsManager.GetDBEngine() == enumDBEngine.MSQLServer.ToString())
                        {
                            optionsBuilder.UseSqlServer(dbCon);
                        }
                        else if (AppSettingsManager.GetDBEngine() == enumDBEngine.MySQl.ToString())
                        {
                            optionsBuilder.UseMySQL(dbCon);

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
