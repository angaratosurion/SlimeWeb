using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class SlimeDbContext : IdentityDbContext
    {
       
       
        public SlimeDbContext(DbContextOptions<SlimeDbContext> options)
                : base(options)
            {
            }
       public DbSet<Blog> Blogs { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Category> Catgories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
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

    }
}
