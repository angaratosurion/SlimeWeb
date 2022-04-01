using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class AccessManager:DataManager
    {
        BlogModsManager blogModsManager = new BlogModsManager();
       BlogManager blogManager = new BlogManager();
        SlimeWebsUserManager userManager = CommonTools.usrmng;
        public async Task<Boolean> DoesUserHasAccess(string username,string blogname)
        {
            try
            {
                Boolean ap = false;
                if (string .IsNullOrEmpty(username)==false && string.IsNullOrEmpty(blogname)==false  
                    && ( userManager.UserExists(username)) && await blogManager.BlogExists(blogname) )
                {
                    var blogbymods = await blogManager.GetBlogActiveModerators(blogname);
                    var blogadm = await blogManager.GetBlogAdministrator(blogname);
                    if ( blogbymods!=null )
                    {
                       
                        var user =  userManager.GetUser(username);
                         
                        if ( blogadm !=null && user!=null)
                        {
                            var blogmod = blogbymods.FirstOrDefault(x => x.UserName == user.UserName  );
                          
                            if ( blogmod!=null ||  blogadm.UserName==user.UserName  || 
                                userManager.UserExistsInRole(SlimeWebsUserManager.AdminRoles,username) )
                            {
                                ap = true;
                            }
                        }
                        else if (user!=null)
                        {
                            if (userManager.UserExistsInRole(SlimeWebsUserManager.AdminRoles, username))
                            {
                                ap = true;
                            }
                        }
                        
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
    }
}
