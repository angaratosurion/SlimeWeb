using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Tools;
using System;

namespace SlimeWeb.Core.Managers
{
    public class InstallManager:DataManager
    {
        Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> AspuserManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> _roleManager;
        public InstallManager(IServiceProvider serviceProvider) 
        {


            AspuserManager = serviceProvider.GetService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            //  RoleStore<ApplicationRole> roleStore = serviceProvider.GetRequiredService<RoleStore<ApplicationRole>>();

        }

        //public InstallManager(SlimeDbContext slimeDbContext) : base(slimeDbContext)
        //{
        //}

        // 
        public void  CrreateInitalAdmin()
        {
            try
            { 
                string adminname=null, adminapss=null;
                adminname = AppSettingsManager.GetDefaultAdminUserName();
                adminapss = AppSettingsManager.GetDefaultAdminUserPassword();
                SlimeWebsUserManager userManager = CommonTools.usrmng;
                if ( userManager != null )
                {
                    if ((!CommonTools.isEmpty(adminname))&& (!CommonTools.isEmpty(adminapss))&&
                        (!userManager.UserExists(adminname)))
                    {
                       var  user = new ApplicationUser
                        {
                            UserName = adminname ,
                            EmailConfirmed = true,
                           Email=adminname
                           
                        };
                        user.DisplayName = "Administrator";
                        user.NormalizedUserName = adminname;
                       
                         userManager.CreateUser(adminname, adminapss);

                    }
                    if(!userManager.RoleExists(SlimeWebsUserManager.AdminRoles))
                    {
                        ApplicationRole adminrol = new ApplicationRole();
                        adminrol.Name = SlimeWebsUserManager.AdminRoles;
                       userManager.CreateNewRole(adminrol);

                    }
                    userManager.AddUserToRole(SlimeWebsUserManager.AdminRoles, adminname);


                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }

        }
    }
}
