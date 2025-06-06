﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Quill.Delta;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
   public   class BlogManager:IDataManager
    {
       
        FileSystemManager flmng = new FileSystemManager();

        
        FileRecordManager fileRecordManager;
        //public BlogManager(SlimeDbContext context):base(context) 
        //{

        //    fileRecordManager = new FileRecordManager(context);
        //}

     

        public virtual async  Task<List<Blog>> ListBlog()
        {
            try
            {
                //return  IDataManager.db.Blogs.ToList();
               
                return  IDataManager.db.Blogs.ToList<Blog>();
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public virtual async Task<List<Blog>> ListBlogBylastUpdated()
        {
            try
            {
                List<Blog> ap = null,tap;
                tap = await this.ListBlog();

                if ( tap!=null)
                {
                    
                   ap=tap.OrderByDescending (x => x.LastUpdate).ToList();
                    //ap = tap;
                }

                



                return ap;


            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public virtual async Task<List<Blog>> ListBlogByAdmUser(string username)
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
        public virtual async Task<List<Blog>> ListBlogByModUser(string username)
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
        public virtual async Task<ViewBlog> GetBlogAsync(string name)
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
        public virtual async Task<ViewBlog> GetBlogByIdAsync(int ?id)
        {
            try
            {
                ViewBlog ap = null;
                //if (!CommonTools.isEmpty(id))
                {
                    ap = new ViewBlog();
                    var tap = (await this.ListBlog()).First(x => x.Id == id);
                    ap.ImportFromModel(tap);
                    if (ap.Categories == null)
                    {
                        ap.Categories = new List<Category>();
                    }
                    if (ap.Posts == null)
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
        public virtual async Task<bool>  BlogExists(string name)
        {
            try
            {
                bool ap = false;
                if (!CommonTools.isEmpty(name))
                {
                    List<Blog> blgs = await this.ListBlog();
                    if(blgs == null)
                    {
                        return false;
                    }
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
        public virtual void CreateBlog(Blog bl, string username)
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
                         IDataManager.db.Add(bl);
                         IDataManager.db.SaveChanges();
                       this.MarkAsUpdated(bl.Name, EntityState.Added);


                        string blpath;

                        //if (CommonTools.isEmpty(blrotfold ))
                        //{
                        //    blrotfold = "Blogfiles";
                        //}
                        // blpath = "~/" + AppDataDir + "/" + blrotfold + "/" + bl.Name;
                        blpath = FileSystemManager.GetBlogRootDataFolderAbsolutePath(bl.Name);
                     //   if (FileSystemManager.DirectoryExists(blpath) == false)
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
        
        public virtual async Task<Blog> EditBasicInfo(Blog bl, string Blogname)
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
                     IDataManager.db.Entry(bl2).CurrentValues.SetValues(bl);
                     IDataManager.db.SaveChanges();
                   await this.MarkAsUpdated(Blogname, EntityState.Modified);
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
        public virtual async Task<List<ApplicationUser>> GetBlogModerators(string Blogname)
        {
            try
            {
                List<ApplicationUser> ap = new List<ApplicationUser>();
              
                if (CommonTools.isEmpty(Blogname) == false
                    && await this.BlogExists(Blogname))
                {
                    Blog bl = await this.GetBlogAsync(Blogname);

                    List<BlogMods> mods =  IDataManager.db.BlogMods.ToList().FindAll(x => x.Id == bl.Id).ToList();
                    if (mods != null)
                    {
                        foreach (var m in mods)
                        {
                            ApplicationUser md = CommonTools.usrmng.GetUserbyID(m.ModeratorId);
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
        public virtual async Task<List<ApplicationUser>> GetBlogActiveModerators(string Blogname)
        {
            try
            {
                List<ApplicationUser> ap = new List<ApplicationUser>();

                if (CommonTools.isEmpty(Blogname) == false
                    && await this.BlogExists(Blogname))
                {
                    Blog bl = await this.GetBlogAsync(Blogname);

                    List<BlogMods> mods =  IDataManager.db.BlogMods.ToList().FindAll(x => x.Id == bl.Id && x.Active==true).ToList();
                    if (mods != null)
                    {
                        foreach (var m in mods)
                        {
                            ApplicationUser md = CommonTools.usrmng.GetUserbyID(m.ModeratorId);
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
        public virtual async Task<ApplicationUser> GetBlogAdministrator(string Blogname)
        {
            try
            {
                ApplicationUser ap = null;

                if (CommonTools.isEmpty(Blogname) == false 
                    && await this.BlogExists(Blogname))
                {
                    Blog bl = (await this.GetBlogAsync(Blogname)).ExportToModel();
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
        public virtual async Task DeleteBlogAsync(string Blogname)
        {
            try
            {
                Blog ap = null;
                if (!CommonTools.isEmpty(Blogname))
                {
                    string path = FileSystemManager.GetBlogRootDataFolderRelativePath(Blogname);
                    List<Files> blfiles = (await this.GetBlogAsync(Blogname)).Files;
                    //if (blfiles != null)
                    //{
                    //    foreach (Files f in blfiles)
                    //    {
                    //        FileSystemManager.DeleteFile(f.RelativePath);
                    //    }
                    //    FileSystemManager.DeleteDirectory(path);
                    //}
                    //if (blfiles != null)
                    //{
                    //     IDataManager.db.Files.RemoveRange(blfiles);
                    //}

                    Blog blog = await this.GetBlogAsync(Blogname);

                    this.fileRecordManager.DeleteByBlog(Blogname);
                    PostManager postManager = new PostManager( );
                    await postManager.DeleteByBlogId(blog.Id);
                    FileSystemManager.DeleteDirectory(path);
                     IDataManager.db.Blogs.Remove(blog); 
                     IDataManager.db.SaveChanges();

                }




            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public virtual async void DeleteBlogByAdm(string username)
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
        public virtual async  Task MarkAsUpdated(string Blogname, EntityState state)
        {
            try
            {
                if(!CommonTools.isEmpty(Blogname))
                {
                   var blog=await this.GetBlogAsync(Blogname);
                    if( blog!=null)
                    {
                        var mblog = blog.ExportToModel();
                        var mblog2=blog.ExportToModel();
                        mblog2.LastUpdate= DateTime.Now;
                         IDataManager.db.Entry(mblog).CurrentValues.SetValues(mblog2);
                         IDataManager.db.Entry(mblog).State = state;
                         IDataManager.db.SaveChanges();

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
