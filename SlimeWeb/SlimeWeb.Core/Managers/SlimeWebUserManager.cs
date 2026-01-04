
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{

    public class SlimeWebsUserManager
    {
        SlimeDbContext db;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> roleManager;
        public SlimeWebsUserManager(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> usrmngr,
            SignInManager<ApplicationUser> singmngr,
            SlimeDbContext tdb, RoleManager<ApplicationRole> roleMgr)
        {
            this._userManager = usrmngr;
            this._signInManager = singmngr;
            roleManager = roleMgr;
            this.Context = tdb;
        }

        public SlimeWebsUserManager() { db = new SlimeDbContext(); }



        public SlimeDbContext Context { get { return db; } set { db = value; } }
        //WikiManager wkmngr = CommonTools.wkmngr;
        public   const string AdminRoles = "Administrators";

        #region User
        public void CreateUser(string username, string testUserPw)
        {
            try
            {


                if (CommonTools.isEmpty(username) == false &&
                    !this.UserExists(username))
                {
                    var user = new ApplicationUser
                    {
                        UserName = username,
                        EmailConfirmed = true,
                        Email = username

                    };

                    user.NormalizedUserName = username;

                    while (this._userManager.CreateAsync(user, testUserPw).Result != IdentityResult.Success)
                    {
                        this._userManager.CreateAsync(user, testUserPw);
                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public ApplicationUser GetUser(string id)
        {
            try
            {
                ApplicationUser ap = null;
                if (id != null)
                {
                    ap = (ApplicationUser)this.db.Users.First(u => u.UserName == id);
                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public ApplicationUser GetUserbyID(string id)
        {
            try
            {
                ApplicationUser ap = null;
                if (id != null)
                {
                    ap = (ApplicationUser)this.db.Users.First(u => u.Id == id);
                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public void EditUser(string username, ApplicationUser user)
        {
            try
            {
                if (CommonTools.isEmpty(username) == false && user != null &&
                    this.UserExists(user.UserName))
                {
                    db.Entry(this.GetUser(username)).CurrentValues.SetValues(user);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public Boolean UserExistsByID(string id)
        {
            try
            {
                Boolean ap = false;
                if (id != null)
                {
                    ApplicationUser us = this.GetUserbyID(id);
                    if (us != null)
                    {
                        ap = true;
                    }
                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public Boolean UserExists(string id)
        {
            try
            {
                Boolean ap = false;
                if (id != null)
                {
                    ApplicationUser us = this.GetUser(id);
                    if (us != null)
                    {
                        ap = true;
                    }
                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }

        public List<ApplicationUser> GetUsers()
        {
            try
            {

                return db.Users.ToList();
                // (List < ApplicationUser > )this.db.Users.ToList();

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public void DeleteUser(string username)
        {
            try
            {
                if (CommonTools.isEmpty(username) == false && this.UserExists(username) == true)
                {
                    ApplicationUser user = this.GetUser(username);
                    List<ApplicationRole> roles = this.GetRolesOfUser(username);
                    if (roles != null)
                    {
                        foreach (var r in roles)
                        {
                            this.RemoveUserFromRole(r.Name, username);

                        }
                    }
                    if (user != null)//&& this._userManager!=null)
                    {

                        this.db.Users.Remove(user);
                        this.db.SaveChanges();

                    }


                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                // return null;
            }
        }


        #endregion
        #region roles
        public Boolean RoleExists(string Name)
        {
            try
            {
                Boolean ap = false;
                if (Name != null)
                {
                    ApplicationRole rol = this.GetRole(Name);
                    if (rol != null)
                    {
                        ap = true;
                    }
                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public ApplicationRole GetRole(string Name)
        {
            try
            {
                ApplicationRole ap = null;
                if (Name != null)
                {
                    //List<ApplicationRole> rols = this.GetRoles();


                    //if (rols != null)
                    //{
                    //    ap = rols.FirstOrDefault(r => r.Name == Name);
                    //}
                    ap = roleManager.FindByNameAsync(Name).Result;
                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public List<ApplicationRole> GetRolesOfUser(string UserName)
        {
            try
            {
                List<ApplicationRole> ap = null, roles;
                if (UserName != null && this.UserExists(UserName))
                {
                    ApplicationUser usr = this.GetUser(UserName);

                    roles = this.GetRoles();
                     if (usr != null &&roles!= null)
                     {
                         ap = new List<ApplicationRole>();
                        var userroles = this.db.UserRoles.ToList();
                        foreach (ApplicationRole rl in roles)
                         {
                            
                            if (userroles != null)
                            {
                                var usrrole = userroles.FindAll(x => x.UserId == usr.Id).ToList();
                                foreach (var rol in usrrole)
                                {


                                    ApplicationRole r = this.db.Roles.FirstOrDefault(x => x.Id == rol.RoleId);
                                    ap.Add(r);
                                }
                            }
                         }
                        

                     }
                    
                }



                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public List<ApplicationRole> GetRoles()
        {
            try
            {
                List<ApplicationRole> ap;
                ap=this.roleManager.Roles.ToList();
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public void CreateNewRole(ApplicationRole role)
        {
            try
            {
                if (role != null && this.RoleExists(role.Name) == false)
                {
                    while (roleManager.CreateAsync(role).Result != IdentityResult.Success)
                    {
                        roleManager.CreateAsync(role);
                        
                    }

                }

                

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                // return null;
            }
        }
        public void EditRole(string rolename, ApplicationRole role)
        {
            try
            {
                if (CommonTools.isEmpty(rolename) == false &&
                    role != null && this.RoleExists(role.Name) == false &&
                    this.RoleExists(rolename))
                {
                    ApplicationRole or = this.GetRole(rolename);
                    if (or != null && or.Name !=SlimeWebsUserManager.AdminRoles
                        && rolename != SlimeWebsUserManager.AdminRoles)
                    {
                        
                        this.db.Entry(or).CurrentValues.SetValues(role);
                        this.db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                // return null;
            }
        }
        public void DeleteRole(string rolename)
        {
            try
            {
                if (CommonTools.isEmpty(rolename) == false)
                {
                    ApplicationRole or = this.GetRole(rolename);
                    if (or != null && rolename != SlimeWebsUserManager.AdminRoles)
                    {
                        this.roleManager.DeleteAsync(or);
                    }
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                // return null;
            }
        }

        public List<ApplicationUser> GetUsersofRole(string Name)
        {
            try
            {
                List<ApplicationUser> ap = null;
                if (Name != null && this.UserExists(Name))
                {
                    ApplicationUser usr = this.GetUser(Name);

                  var  roles = this.GetRoles();
                    if (usr != null && roles != null)
                    {
                        ap = _userManager.GetUsersInRoleAsync(Name).Result.ToList();
                        
                    }

                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async void AddUserToRole(string rolename, string username)
        {
            try
            {
                    if (CommonTools.isEmpty(rolename) == false
                        && CommonTools.isEmpty(username) == false &&
                 this.RoleExists(rolename) && this.UserExists(username) == true)
                {
                        ApplicationRole or = this.GetRole(rolename);
                        ApplicationUser user = this.GetUser(username);
                    if (this.UserExistsInRole(rolename, username) == false)
                    {


                     //while(this._userManager.AddToRoleAsync(user, rolename).Result!=IdentityResult.Success) 
                        {
                             this._userManager.AddToRoleAsync(user, rolename).Wait();
                        }
                    }


                
                    }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                // return null;
            }
        }
        public void RemoveUserFromRole(string rolename, string username)
        {
            try
            {
                if (CommonTools.isEmpty(rolename) == false
                    && CommonTools.isEmpty(username) == false &&
                    this.RoleExists(rolename) && this.UserExists(username) == true)
                {
                    ApplicationRole or = this.GetRole(rolename);
                    ApplicationUser user = this.GetUser(username);
                    if (this.UserExistsInRole(rolename, username) != false)
                    {
                        _userManager.RemoveFromRoleAsync(user, rolename);
                    }

                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                // return null;
            }
        }
        public Boolean UserExistsInRole(string rolename, string username)
        {
            try
            {
                Boolean ap = false;
                if (CommonTools.isEmpty(rolename) == false
                     && CommonTools.isEmpty(username) == false &&
                     this.RoleExists(rolename) && this.UserExists(username) == true)
                {
                    ApplicationUser us = this.GetUser(username);
                   
                    ap= _userManager.IsInRoleAsync(us, username).Result;

                }

                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }

        #endregion

        #region claims



        #endregion
    }
}

   
