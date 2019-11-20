using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.Models;

namespace SlimeWeb.Data
{
    public class ApplicationDbContext : SlimeDbContext////IdentityDbContext
   // public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<SlimeDbContext> options)
           : base(options)
        {
        }

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //   : base(options)
        //{
        //}
    }
}
