using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class InstallManager:DataManager
    {
        Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> AspuserManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> _roleManager;
        public InstallManager(IServiceProvider serviceProvider) 
        {


            AspuserManager = serviceProvider.GetService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();

          //  RoleStore<ApplicationRole> roleStore = serviceProvider.GetRequiredService<RoleStore<ApplicationRole>>();
            
        }
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
                        AspuserManager.CreateAsync(user, adminapss);

                    }
                    if(!userManager.RoleExists(SlimeWebsUserManager.AdminRoles))
                    {
                        ApplicationRole adminrol = new ApplicationRole();
                        adminrol.Name = SlimeWebsUserManager.AdminRoles;
                        //var  res=_roleManager.CreateAsync(adminrol).Result;
                        adminrol.Id = SlimeWebsUserManager.AdminRoles;
                        db.Roles.Add(adminrol);
                        db.SaveChangesAsync();

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
