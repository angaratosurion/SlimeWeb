using Microsoft.AspNetCore.Identity;
using SlimeWeb.Core.Data.Models;
using System.Collections.Generic;

namespace SlimeWeb.Core.Data.NonDataModels.UserAndRoles
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }
}
