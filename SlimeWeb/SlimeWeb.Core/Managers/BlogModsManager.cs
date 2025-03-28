using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class BlogModsManager : IDataManager//IBlogModsManager
    {
        BlogManager blmngr = new BlogManager();
        SlimeWebsUserManager userManager = CommonTools.usrmng;

        

        public async Task<List<BlogMods>> ListMods()
        {
            try
            {
                List<BlogMods> ap = null;


                ap = IDataManager.db.BlogMods.ToList();



                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<BlogMods> Details(string blogname, string modname)
        {
            try
            {
                BlogMods ap = null;
                Boolean blogexists = await blmngr.BlogExists(blogname);
                if (!CommonTools.isEmpty(blogname) && (blogexists))
                {
                    List<BlogMods> mods = (await this.ListModsByBlogName(blogname)).ToList();
                    ap = mods.Find(x => x.ModeratorId == modname);



                }



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
                Boolean blogexists = await blmngr.BlogExists(blogname);
                if (!CommonTools.isEmpty(blogname) && (blogexists))
                {
                    Blog blog = await blmngr.GetBlogAsync(blogname);
                    List<BlogMods> tap = await this.ListMods();

                    if (tap != null && (tap.Where(x => x.BlogId == blog.Id).ToList() != null))
                    {
                        var mods = tap.Where(x => x.BlogId == blog.Id).ToList();
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
        public async Task<BlogMods> ListModByBlogId(int id)
        {
            try
            {
                BlogMods ap = null;



                List<BlogMods> tap = await this.ListMods();

                if (tap != null && tap.Find(x => x.Id == id) != null)
                {
                    var mods = tap.Find(x => x.Id == id);
                    ap = mods;
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
                if (CommonTools.isEmpty(modname) != false && userManager.GetUser(modname) != null)
                {
                    ApplicationUser applicationUser = userManager.GetUser(modname);
                    List<BlogMods> tap = await this.ListMods();

                    if (tap != null && (tap.Where(x => x.ModeratorId == applicationUser.UserName).ToList() != null))
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
        public async void RegisterMods(string blogname, string modname)
        {
            try
            {
                if (!CommonTools.isEmpty(blogname) && !CommonTools.isEmpty(modname)
                    && (await blmngr.BlogExists(blogname))
                    && userManager.UserExists(modname))
                {
                    var blog = await blmngr.GetBlogAsync(blogname);
                    var user = userManager.GetUser(modname);
                    BlogMods blogmod = new BlogMods();
                    blogmod.BlogId = blog.ExportToModel().Id;
                    blogmod.ModeratorId = user.UserName;
                    blogmod.Active = false;
                     IDataManager.db.BlogMods.Add(blogmod);
                    await  IDataManager.db.SaveChangesAsync();
                    await this.blmngr.MarkAsUpdated(blogname, EntityState.Modified);


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
                        var blog = await blmngr.GetBlogAsync(blogname);
                        var blogmod = lstblogmod.First(x => x.BlogId == blog.ExportToModel().Id);
                        if (blogmod != null)
                        {
                             IDataManager.db.BlogMods.Remove(blogmod);
                            await  IDataManager.db.SaveChangesAsync();
                            await this.blmngr.MarkAsUpdated(blogname, EntityState.Modified);

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
        public async Task<BlogMods> Edit(string modname, BlogMods mods, string blogname)
        {
            try
            {
                BlogMods vmods = null;
                if (mods != null && !CommonTools.isEmpty(blogname) && !CommonTools.isEmpty(modname))
                {


                    var lstmods = await this.ListModsByBlogName(blogname);
                    if (lstmods != null)
                    {
                        vmods = lstmods.First(x => x.ModeratorId == modname);
                        if (vmods != null)
                        {
                            mods.BlogId = vmods.BlogId;

                             IDataManager.db.Entry(vmods).State = EntityState.Modified;
                            mods.Id = vmods.Id;
                            mods.BlogId = vmods.BlogId;

                             IDataManager.db.Entry(vmods).CurrentValues.SetValues(mods);
                            //  IDataManager.db.Post.Update(Post);
                            await  IDataManager.db.SaveChangesAsync();
                        }
                    }
                }
                return mods;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is BlogMods)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            // TODO: decide which value should be written to database
                            proposedValues[property] = proposedValue;
                        }

                        // Refresh original values to bypass next concurrency check
                        // entry.OriginalValues.SetValues(databaseValues);
                        entry.OriginalValues.SetValues(proposedValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            

        }

        }
    }
}
