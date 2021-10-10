using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class BlogModsManager : DataManager
    {
        BlogManager blmngr = new BlogManager();
        SlimeWebsUserManager userManager = CommonTools.usrmng;
        public async Task<List<BlogMods>> ListMods()
        {
            try
            {
                List<BlogMods> ap = null;


                 ap = DataManager.db.BlogMods.ToList();
               


                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task< BlogMods >Details(int id)
        {
            try
            {
                 BlogMods ap = null;


                ap = (await this.ListMods()).FirstOrDefault(x => x.Id==id);



                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<List<BlogMods>> ListModsByBlogName(string blogname)
        {
            try
            {
                List<BlogMods> ap = null;
                if(CommonTools.isEmpty(blogname)!=false && (await blmngr.BlogExists(blogname)))
                {
                    Blog blog = await blmngr.GetBlogAsync(blogname);
                    List<BlogMods> tap =await this.ListMods();
                    
                    if ( tap!=null && (tap.Where(x => x.BlogId == blog.Id).ToList()!=null))
                    {
                        var mods= tap.Where(x => x.BlogId == blog.Id).ToList();
                        ap = mods;
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
        public async Task<List<BlogMods>> ListModsByModName(string modname)
        {
            try
            {
                List<BlogMods> ap = null;
                if (CommonTools.isEmpty(modname) != false &&  userManager.GetUser(modname)!=null)
                {
                    ApplicationUser applicationUser = userManager.GetUser(modname);
                    List<BlogMods> tap = await this.ListMods();

                    if (tap != null && (tap.Where(x => x.ModeratorId==applicationUser.UserName).ToList() != null))
                    {
                        var mods = tap.Where(x => x.ModeratorId == applicationUser.UserName).ToList();
                        ap = mods;
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
        public async void RegisterMods(string blogname,string modname)
        {
            try
            {
                if(CommonTools.isEmpty(blogname) != false && CommonTools.isEmpty(modname) != false && (await blmngr.BlogExists(blogname))
                    && (userManager.UserExists(modname)))
                {
                    var blog =await  blmngr.GetBlogAsync(blogname);
                    var user = userManager.GetUser(modname);
                    BlogMods blogmod = new BlogMods();
                    blogmod.BlogId = blog.ExportToModel().Id;
                    blogmod.ModeratorId = user.UserName;
                    blogmod.Active = false;
                    db.BlogMods.Add(blogmod);
                    await db.SaveChangesAsync();


                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
               // return null;
            }
        }
        public async void UnRegisterMods(string blogname, string modname)
        {
            try
            {
                if (CommonTools.isEmpty(blogname) != false && CommonTools.isEmpty(modname) != false && (await blmngr.BlogExists(blogname))
                    && (userManager.UserExists(modname)))
                {
                    var lstblogmod = (await this.ListModsByModName(modname)).ToList();
                    if (lstblogmod != null)
                    {
                        var blog =await blmngr.GetBlogAsync(blogname);
                        var blogmod = lstblogmod.First(x => x.BlogId==blog.ExportToModel().Id);
                        if (blogmod != null)
                        {
                            db.BlogMods.Remove(blogmod);
                            await db.SaveChangesAsync();
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

    }
}
