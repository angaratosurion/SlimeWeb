using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityResult = Microsoft.AspNet.Identity.IdentityResult;

namespace SlimeWeb.Core.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "UserName")]
        public string DisplayName { get; set; }
        public ApplicationUser Clone()
        {
            try
            {
                ApplicationUser ap = new ApplicationUser();

                //if ( this !=null)
                { 
                    //ap.Id = this.Id;
                    ap.UserName = this.UserName;
                    ap.AccessFailedCount = this.AccessFailedCount;
                    ap.LockoutEnabled = this.LockoutEnabled;
                    //ap.LockoutEndDateUtc = this.LockoutEndDateUtc;
                    ap.PasswordHash = this.PasswordHash;
                    ap.PhoneNumber = this.PhoneNumber;
                    ap.PhoneNumberConfirmed = this.PhoneNumberConfirmed;
                    ap.SecurityStamp = this.SecurityStamp;
                    ap.TwoFactorEnabled = this.TwoFactorEnabled;
                    ap.Email = this.Email;
                    ap.EmailConfirmed = this.EmailConfirmed;
                    //var roles = this.Roles;

                    // foreach( var rol in roles)
                    //{
                    //    ap.Roles.Add(rol);
                    //}
                    //var claims = this.Claims;
                    //foreach (var claim in claims)
                    //{
                    //    ap.Claims.Add(claim);
                    //}
                    //var logins = this.Logins;
                    //foreach (var login in logins)
                    //{
                    //    ap.Logins.Add(login);
                    //}

                }
                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<Microsoft.AspNetCore.Identity.IdentityResult> GenerateUserIdentityAsync(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var thisIdentity = await manager.CreateAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom this claims here

            return thisIdentity;
        }

    }
}
