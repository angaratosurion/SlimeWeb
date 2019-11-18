using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.Models;

namespace SlimeWeb.Data
{
    public class ApplicationDbContext : SlimeDbContentext//IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<SlimeDbContentext> options)
           : base(options)
        {
        }
    }
}
