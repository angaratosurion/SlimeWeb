using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Owin;
using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeWeb.Core.Managers
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        //public ApplicationUserManager(IUserStore<ApplicationUser> store)
        //   // : base(store)
        //{
        //}

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = this;// new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<SlimeDbContext>()));
            // Configure validation logic for usernames
           
            // Configure validation logic for passwords
            PasswordValidator<ApplicationUser> paval = new Microsoft.AspNetCore.Identity.PasswordValidator<ApplicationUser>();
            paval.Describer.PasswordRequiresDigit();
            paval.Describer.PasswordRequiresNonAlphanumeric();
            paval.Describer.PasswordRequiresUpper();
            paval.Describer.PasswordTooShort(6);
            paval.Describer.PasswordRequiresLower();
            manager.PasswordValidators.Add(paval);
            {
                //RequiredLength = 6,
                
               
                
            };

            // Configure user lockout defaults
           /* manager.UserLockoutEnabledByDefault = false;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;*/


            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
          
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                
            }
            return manager;
        }
    }
}
