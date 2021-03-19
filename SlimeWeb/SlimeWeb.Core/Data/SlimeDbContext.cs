using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data
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
                return this.Settings.FirstOrDefault();
            }
        }
        public DbSet<BannedUsers> BannedUsers { get; set; }
        public DbSet<News> News{ get; set; }
        public DbSet<BlogMods>  BlogMods { get; set; }

        public DbSet<CategotyPost> CategoryPosts  { get; set; }
        public DbSet<TagPost> TagPosts { get; set; }
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



            string pathwithextention= System.Reflection.Assembly.GetExecutingAssembly().CodeBase;


            string path = System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\","");
            
                          
                            
             
            var directory = Path.Combine(path, "App_Data");
            if(Directory.Exists(directory)== false)
            {
                Directory.CreateDirectory(directory);
            }
            string olddbConn = AppSettingsManager.GetDefaultConnectionString();
            if (olddbConn != null)
            {

                string dbCon = olddbConn.Replace("|DataDirectory|", directory);
                if (dbCon != null)
                {
                    optionsBuilder.UseSqlServer(dbCon);
                    
                }
            }
               

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                 .Property(e => e.Id)
                .ValueGeneratedOnAdd();

        }

    }
}
