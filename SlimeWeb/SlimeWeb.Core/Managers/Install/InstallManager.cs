using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;
using System;

namespace SlimeWeb.Core.Managers.Install
{
    public class InstallManager : IDataManager
    {
        SlimeDbContext db;
        UserManager<ApplicationUser> AspuserManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public InstallManager(IServiceProvider serviceProvider)
        {


            AspuserManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            _signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            db = serviceProvider.GetService<SlimeDbContext>();
            //  RoleStore<ApplicationRole> roleStore = serviceProvider.GetRequiredService<RoleStore<ApplicationRole>>();

        }

        //public InstallManager(SlimeDbContext slimeDbContext) : base(slimeDbContext)
        //{
        //}

        // 
        public void CrreateInitalAdmin()
        {
            try
            {
                string adminname = null, adminapss = null;
                adminname = AppSettingsManager.GetDefaultAdminUserName();
                adminapss = AppSettingsManager.GetDefaultAdminUserPassword();
                 
                SlimeWebsUserManager userManager = new SlimeWebsUserManager(AspuserManager,
                    _signInManager, db, _roleManager);
                if (userManager != null)
                {
                    if (!CommonTools.isEmpty(adminname) && !CommonTools.isEmpty(adminapss) &&
                        !userManager.UserExists(adminname))
                    {
                        var user = new ApplicationUser
                        {
                            UserName = adminname,
                            EmailConfirmed = true,
                            Email = adminname

                        };
                        user.DisplayName = "Administrator";
                        user.NormalizedUserName = adminname;

                        userManager.CreateUser(adminname, adminapss);

                    }
                    if (!userManager.RoleExists(SlimeWebsUserManager.AdminRoles))
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
