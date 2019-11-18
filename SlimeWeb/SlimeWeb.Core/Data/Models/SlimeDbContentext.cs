using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeWeb.Core.Data.Models
{
    public class SlimeDbContentext : IdentityDbContext
    {     
            public SlimeDbContentext(DbContextOptions<SlimeDbContentext> options)
                : base(options)
            {
            }
       
    }
}
