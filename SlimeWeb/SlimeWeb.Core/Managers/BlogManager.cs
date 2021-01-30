using Microsoft.AspNetCore.Hosting;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
   public   class BlogManager
    {
       
        FileSystemManager flmng = new FileSystemManager();

        SlimeDbContext slimeDb = new SlimeDbContext();
        public async  Task<List<Blog>> ListBlog()
        {
            try
            {
                //return this.db.Blogs.ToList();
                return this.slimeDb.Blogs.ToList<Blog>();
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<List<Blog>> ListBlogByAdmUser(string username)
        {
            try
            {
                List<Blog> ap = null;
                if (!CommonTools.isEmpty(username) && CommonTools.usrmng.UserExists(username))
                {
                    var adm = CommonTools.usrmng.GetUser(username);
                    if ( adm==null)
                    {
                        return null;
                    }
                    ap = (await this.ListBlog()).FindAll(x => x.Administrator == adm.Id);
                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<List<Blog>> ListBlogByModUser(string username)
        {
            try
            {
                List<Blog> ap = null;
                if (!CommonTools.isEmpty(username) && CommonTools.usrmng.UserExists(username))
                {
                    //ap = this.ListBlog().FindAll(x => x.Moderators.First(x=>x.Moderator == username));
                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<ViewBlog> GetBlogAsync(string name)
        {
            try
            {
                ViewBlog ap = null;
                if (!CommonTools.isEmpty(name))
                {
                    ap = new ViewBlog();
                    var tap = (await this.ListBlog()).First(x => x.Name==name);
                    ap.ImportFromModel(tap);
                    if (ap.Categories == null)
                    {
                        ap.Categories = new List<Category>();
                    }
                    if (ap.Posts== null)
                    {
                        ap.Posts = new List<Post>();
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
        public async Task<bool>  BlogExists(string name)
        {
            try
            {
                bool ap = false;
                if (!CommonTools.isEmpty(name))
                {
                    List<Blog> blgs = await this.ListBlog();
                    if(blgs.Find(x => x.Name == name)!=null)
                    {
                        ap = true;
                    }

                }


                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false; ;
            }
        }
        public void CreateBlog(Blog bl, string username)
        {
            try
            {
                ApplicationUser usr = null;

                if (bl != null && CommonTools.isEmpty(username) == false)
                {



                    usr = CommonTools.usrmng.GetUser(username);
                    if (usr != null)
                    {
                        bl.Administrator = usr.Id;//.Clone();
                                                  //bl.AdministratorId = bl.Administrator.Id;



                        //  bl.Moderators = new List<BlogMods>();
                        //  BlogMods wm = new BlogMods();
                        // wm.Blog = bl;
                        // wm.Moderator = usr.Id;
                        //bl.Moderators.Add(wm);
                        this.slimeDb.Add(bl);
                        this.slimeDb.SaveChanges();

                       
                        string blpath;

                        //if (CommonTools.isEmpty(blrotfold ))
                        //{
                        //    blrotfold = "Blogfiles";
                        //}
                        // blpath = "~/" + AppDataDir + "/" + blrotfold + "/" + bl.Name;
                        blpath = FileSystemManager.GetBlogRootDataFolderRelativePath(bl.Name);
                        if (FileSystemManager.DirectoryExists(blpath) == false)
                        {
                            FileSystemManager.CreateDirectory(blpath);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        
        public async Task<Blog> EditBasicInfo(Blog bl, string Blogname)
        {
            try
            {
                Blog ap = null, bl2;
                if (bl != null && CommonTools.isEmpty(Blogname) == false)
                {



                    bl2 = (await this.GetBlogAsync(Blogname)).ExportToModel();
                    bl.Administrator = bl2.Administrator;
                    bl.Id = bl2.Id;
                    bl.LastUpdate = DateTime.Now;
                    slimeDb.Entry(bl2).CurrentValues.SetValues(bl);
                    slimeDb.SaveChanges();
                    ap =(await  this.GetBlogAsync(Blogname)).ExportToModel();
                }


                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<List<ApplicationUser>> GetBlogModerators(string Blogname)
        {
            try
            {
                List<ApplicationUser> ap = new List<ApplicationUser>();
              
                if (CommonTools.isEmpty(Blogname) == false
                    && await this.BlogExists(Blogname))
                {
                    Blog bl = await this.GetBlogAsync(Blogname);

                    List<BlogMods> mods = this.slimeDb.BlogMods.ToList().FindAll(x => x.Id == bl.Id).ToList();
                    if (mods != null)
                    {
                        foreach (var m in mods)
                        {
                            ApplicationUser md = CommonTools.usrmng.GetUserbyID(m.Moderator);
                            if (md != null)
                            {
                                ap.Add(md);
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
        public async Task<ApplicationUser> GetBlogAdministrator(string Blogname)
        {
            try
            {
                ApplicationUser ap = null;

                if (CommonTools.isEmpty(Blogname) == false 
                    && await this.BlogExists(Blogname))
                {
                    Blog bl = await this.GetBlogAsync(Blogname);
                    string adm = bl.Administrator;
                  //  if (CommonTools.isEmpty(adm) == false)
                    {
                        ap = CommonTools.usrmng.GetUserbyID(adm);

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
        public async Task DeleteBlogAsync(string Blogname)
        {
            try
            {
                Blog ap = null;
                if (!CommonTools.isEmpty(Blogname))
                {
                    string path = FileSystemManager.GetBlogRootDataFolderRelativePath(Blogname);
                    List<Files> blfiles = (await this.GetBlogAsync(Blogname)).Files;
                    if (blfiles != null)
                    {
                        foreach (Files f in blfiles)
                        {
                            FileSystemManager.DeleteFile(f.RelativePath);
                        }
                        FileSystemManager.DeleteDirectory(path);
                    }
                    if (blfiles != null)
                    {
                        this.slimeDb.Files.RemoveRange(blfiles);
                    }
                    Blog blog = await this.GetBlogAsync(Blogname);
                    this.slimeDb.Blogs.Remove(blog); 
                    this.slimeDb.SaveChanges();

                }




            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public async void DeleteBlogByAdm(string username)
        {
            try
            {
                Blog ap = null;
                if (!CommonTools.isEmpty(username))
                {

                    List<Blog> bls = await this.ListBlogByAdmUser(username);
                    if (bls != null)
                    {
                        foreach (Blog w in bls)
                        {
                            this.DeleteBlogAsync(w.Name);
                        }
                    }


                }




            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
    }
}
