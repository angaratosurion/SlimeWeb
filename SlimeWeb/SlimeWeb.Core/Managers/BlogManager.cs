using Microsoft.AspNetCore.Hosting;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SlimeWeb.Core.Managers
{
   public  class BlogManager
    {
       
        FileSystemManager flmng = new FileSystemManager();

        SlimeDbContext slimeDb = new SlimeDbContext();
        public List<Blog> ListBlog()
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
        public List<Blog> ListBlogByAdmUser(string username)
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
                    ap = this.ListBlog().FindAll(x => x.Administrator == adm.Id);
                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public List<Blog> ListBlogByModUser(string username)
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
        public Blog GetBlog(string name)
        {
            try
            {
                Blog ap = null;
                if (!CommonTools.isEmpty(name))
                {
                    ap = this.ListBlog().First(x => x.Name==name);
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
        public bool  BlogExists(string name)
        {
            try
            {
                bool ap = false;
                if (!CommonTools.isEmpty(name))
                {
                    List<Blog> blgs = this.ListBlog();
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
        
        public Blog EditBasicInfo(Blog bl, string Blogname)
        {
            try
            {
                Blog ap = null, bl2;
                if (bl != null && CommonTools.isEmpty(Blogname) == false)
                {

                

                      bl2 = this.GetBlog(Blogname);
                    bl.Administrator = bl2.Administrator;
                    slimeDb.Entry(this.GetBlog(Blogname)).CurrentValues.SetValues(bl);
                    slimeDb.SaveChanges();
                    ap = this.GetBlog(Blogname);
                }


                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public List<ApplicationUser> GetBlogModerators(string Blogname)
        {
            try
            {
                List<ApplicationUser> ap = new List<ApplicationUser>();

                if (CommonTools.isEmpty(Blogname) == false && this.BlogExists(Blogname))
                {
                    Blog bl = this.GetBlog(Blogname);

                    List<BlogMods> mods = bl.Moderators;
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
        public ApplicationUser GetBlogAdministrator(string Blogname)
        {
            try
            {
                ApplicationUser ap = null;

                if (CommonTools.isEmpty(Blogname) == false && this.BlogExists(Blogname))
                {
                    Blog bl = this.GetBlog(Blogname);
                    int adm = bl.Administrator;
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
        public void DeleteBlog(string Blogname)
        {
            try
            {
                Blog ap = null;
                if (!CommonTools.isEmpty(Blogname))
                {
                    string path = FileSystemManager.GetBlogRootDataFolderRelativePath(Blogname);
                    List<Files> blfiles = this.GetBlog(Blogname).Files;
                    if (blfiles != null)
                    {
                        foreach (Files f in blfiles)
                        {
                            FileSystemManager.DeleteFile(f.RelativePath);
                        }
                        FileSystemManager.DeleteDirectory(path);
                    }

                    this.slimeDb.Files.RemoveRange(blfiles);
                    this.slimeDb.Blogs.Remove(this.GetBlog(Blogname));
                    this.slimeDb.SaveChanges();

                }




            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public void DeleteBlogByAdm(string username)
        {
            try
            {
                Blog ap = null;
                if (!CommonTools.isEmpty(username))
                {

                    List<Blog> bls = this.ListBlogByAdmUser(username);
                    if (bls != null)
                    {
                        foreach (Blog w in bls)
                        {
                            this.DeleteBlog(w.Name);
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
