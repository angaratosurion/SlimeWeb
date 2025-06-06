﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class FileRecordManager : IDataManager
    {
        BlogManager blogmngr;


        PostManager  postManager;
        SlimeWebPageManager pageManager;
      
        public FileRecordManager()
        {
           
            postManager = new PostManager ( );
            blogmngr = new BlogManager( );
            pageManager= new SlimeWebPageManager( );
        }

        public virtual async  Task<List<Files>> GetFiles()
        {
            try
            {
                List<Files> ap = null;

                ap =  IDataManager.db.Files.ToList();
                       

                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public virtual async  Task<string> CreateForBlog(int? BlogId, int? postid, 
            Files filemodel, IFormFile filedata,string user)
        {
            try
            { string ap = null;
                if((filedata!=null) && (filemodel!=null) && (!CommonTools.isEmpty(user )))
                {
                    var blog = await blogmngr.GetBlogByIdAsync(BlogId);
                    var post = await postManager.Details(postid);
                    ApplicationUser usr = (ApplicationUser) IDataManager.db.Users.First(m => m.UserName == user);
                    if ( (blog !=null) )
                    {
                        var blogpath = FileSystemManager.GetBlogRootDataFolderAbsolutePath(blog.Name);
                       string  abspath=await  FileSystemManager.CreateFile(blogpath, filedata);
                        if (!CommonTools.isEmpty(abspath))
                        {
                            ap = FileSystemManager.GetBlogRootDataFolderRelativePath(blog.Name) + "/" + Path.GetFileName(abspath);
                            filemodel.FileName = Path.GetFileName(abspath);
                            filemodel.Path = abspath;
                            filemodel.RelativePath = ap;
                            filemodel.ContentType = filedata.ContentType; 
                            filemodel.Owner = usr.UserName;
                            //filemodel.PostId =(int) postid;
                           
                             IDataManager.db.Files.Add(filemodel);
                           await   IDataManager.db.SaveChangesAsync();
                            FilesPostBlog filesPost = new FilesPostBlog();
                            filesPost.BlogId = blog.Id;
                            filesPost.FileId = filemodel.Id;
                            filesPost.PostId = (int)postid;
                             IDataManager.db.FilesPostsBlog.Add(filesPost);
                            await  IDataManager.db.SaveChangesAsync();
                            await this.blogmngr.MarkAsUpdated(blog.Name, EntityState.Modified);

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
        public virtual async  Task<string> CreateForPage(string pagename, Files filemodel,
            IFormFile filedata, string user)
        {
            try
            {
                string ap = null;
                if ((filedata != null) && (filemodel != null) && (!CommonTools.isEmpty(user)))
                {
                    var page = await pageManager.Details(pagename);
                    
                    ApplicationUser usr = (ApplicationUser) IDataManager.db.Users.First(m => m.UserName == user);
                    if ((page != null))
                    {

                        var blogpath = FileSystemManager.GetPagesRootDataFolderAbsolutePath(pagename);
                        
                        if (FileSystemManager.DirectoryExists(blogpath) == false)
                        {
                            FileSystemManager.CreateDirectory(blogpath);
                        }
                        string abspath = await FileSystemManager.CreateFile(blogpath, filedata);
                        if (!CommonTools.isEmpty(abspath))
                        {
                            ap = FileSystemManager.GetPagesRootDataFolderRelativePath(pagename) + "/" + Path.GetFileName(abspath);
                            filemodel.FileName = Path.GetFileName(abspath);
                            filemodel.Path = abspath;
                            filemodel.RelativePath = ap;
                            filemodel.ContentType = filedata.ContentType;
                            filemodel.Owner = usr.UserName;
                            //filemodel.PostId =(int) postid;

                             IDataManager.db.Files.Add(filemodel);
                            await  IDataManager.db.SaveChangesAsync();
                            FilesPages filesPost = new FilesPages();
                           
                            filesPost.FileId = filemodel.Id;
                            filesPost.PageId = page.Id;
                             IDataManager.db.FilesPages.Add(filesPost);
                            await  IDataManager.db.SaveChangesAsync();

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
        public virtual async  Task<string> CreateForPage(int pid, Files filemodel,
            IFormFile filedata, string user)
        {
            try
            {
                string ap = null;
                if ((filedata != null) && (filemodel != null) && (!CommonTools.isEmpty(user)))
                {
                    var page = await pageManager.Details(pid);

                    ApplicationUser usr = (ApplicationUser) IDataManager.db.Users.First(m => m.UserName == user);
                    if ((page != null))
                    {
                        var blogpath = FileSystemManager.GetPagesRootDataFolderAbsolutePath(page.Name) ;
                        if (FileSystemManager.DirectoryExists(blogpath) == false)
                        {
                            FileSystemManager.CreateDirectory(blogpath );
                        }

                            string abspath = await FileSystemManager.CreateFile(blogpath, filedata);
                        if (!CommonTools.isEmpty(abspath))
                        {
                            ap = FileSystemManager.GetPagesRootDataFolderRelativePath(page.Name) + "/" + Path.GetFileName(abspath);
                            filemodel.FileName = Path.GetFileName(abspath);
                            filemodel.Path = abspath;
                            filemodel.RelativePath = ap;
                            filemodel.ContentType = filedata.ContentType;
                            filemodel.Owner = usr.UserName;
                            //filemodel.PostId =(int) postid;

                             IDataManager.db.Files.Add(filemodel);
                            await  IDataManager.db.SaveChangesAsync();
                            FilesPages filesPost = new FilesPages();

                            filesPost.FileId = filemodel.Id;
                            filesPost.PageId = page.Id;
                             IDataManager.db.FilesPages.Add(filesPost);
                            await  IDataManager.db.SaveChangesAsync();

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
        public virtual async   Task<List<Files>> GetFilesByBlogName(string BlogName)
        {
            try
            {
                List<Files> ap = new List<Files>();
                if ( (!CommonTools.isEmpty(BlogName) )   &&(await blogmngr.BlogExists(BlogName)))
                {
                    var blog = await blogmngr.GetBlogAsync(BlogName);
                    
                    if( blog !=null )
                    {
                        var filesblog =  IDataManager.db.FilesPostsBlog.Where(x => x.BlogId == blog.Id).ToList();
                        if (filesblog != null)
                        {
                            foreach (var fb in filesblog)
                            {
                                var file = await this.Details(fb.FileId);
                                if (file != null)
                                {
                                    ap.Add(file);
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
        public virtual async  Task<List<Files>> GetFilesByBlogId(int id)
        {
            try
            {
                List<Files> ap = new List<Files>();

                var blog = await blogmngr.GetBlogByIdAsync(id);

                    if (blog != null)
                    {
                    var filesblog =  IDataManager.db.FilesPostsBlog.Where(x => x.BlogId == id).ToList();
                    if (filesblog != null)
                    {
                        foreach (var fb in filesblog)
                        {
                            var file = await this.Details(fb.FileId);
                            if (file != null)
                            {
                                ap.Add(file);
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
        public virtual async  Task<List<Files>> GetFilesByPostId(int pid)
        {
            try
            {
                List<Files> ap = new List<Files>();

                var post = await postManager.Details(pid);

                if (post!= null)
                {
                    var fileposts =  IDataManager.db.FilesPostsBlog.Where(x => x.PostId == post.Id).ToList();
                    if (fileposts!=null)
                    {
                        //ap =  IDataManager.db.Files.Where(x => x.PostId== post.Id).ToList();
                        foreach( var filepost in fileposts)
                        {
                            var file = await this.Details(filepost.FileId);
                             if (file!=null)
                            {
                                ap.Add(file);
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
        public virtual async  Task<Files> Details(int id)
        {
            try
            {
                Files ap = null;

                ap =  IDataManager.db.Files.FirstOrDefault(x => x.Id == id);

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public virtual async  Task<bool> Delete(int id)
        {
            try
            {

                bool ap = false;
                //   IDataManager.db.Files.FirstOrDefault(x => x.Id == id);
                var file = await this.Details(id);
                if (file!=null)
                {
                    bool deleted=FileSystemManager.DeleteFile(file.Path);

                    if (deleted)
                    {
                         IDataManager.db.Files.Remove(file);
                        await  IDataManager.db.SaveChangesAsync();
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
        public virtual async  Task<bool> DeleteFromPosts(int id)
        {
            try
            {

                bool ap = false;
                //   IDataManager.db.Files.FirstOrDefault(x => x.Id == id);
                var file = await this.Details(id);
                if (file != null)
                {
                    bool deleted = FileSystemManager.DeleteFile(file.Path);

                    if (deleted)
                    {
                         IDataManager.db.Files.Remove(file);
                        var filear =  IDataManager.db.FilesPostsBlog.Where(x => x.FileId == id).ToList();
                        if (filear != null)
                        {
                            foreach (var x in filear)
                            {
                                IDataManager.db.FilesPostsBlog.Remove(x);
                                await this.blogmngr.MarkAsUpdated((await blogmngr.GetBlogByIdAsync(x.BlogId)).Name, EntityState.Modified);


                            }
                        }
                        await  IDataManager.db.SaveChangesAsync();
                        
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
        public virtual async  Task<bool> DeleteFromPages(int id)
        {
            try
            {

                bool ap = false;
                //   IDataManager.db.Files.FirstOrDefault(x => x.Id == id);
                var file = await this.Details(id);
                if (file != null)
                {
                    bool deleted = FileSystemManager.DeleteFile(file.Path);

                    if (deleted)
                    {
                         IDataManager.db.Files.Remove(file);
                        var filear =  IDataManager.db.FilesPages.Where(x => x.FileId == id).ToList();
                        if (filear != null)
                        {
                            foreach (var x in filear)
                            {
                                 IDataManager.db.FilesPages.Remove(x);


                            }
                        }
                        await  IDataManager.db.SaveChangesAsync();
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
        public virtual async  Task<bool> DeleteByPostId(int pid)
        {
            try
            {
                bool ap = false;


                var files = await this.GetFilesByPostId(pid);
                if (files != null)
                {
                    foreach (var file in files)
                    {
                       ap = await this.Delete(file.Id);
                    }
                }
                await this.blogmngr.MarkAsUpdated((await blogmngr.GetBlogByIdAsync((await postManager.Details(pid)).BlogId)).Name, EntityState.Modified);

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;

            }
        }
        public virtual async  void DeleteByBlog(string blogname)
        {
            try
            {



                var files = await this.GetFilesByBlogName(blogname);
                if (files != null)
                {
                    foreach(var file in files)
                    {
                       await this.Delete(file.Id);
                    }
                }



            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }

        public virtual async  Task<bool> PostHasFiles(int id)
        {
            try
            {

                bool ap = false;
                //   IDataManager.db.Files.FirstOrDefault(x => x.Id == id);
                var file = await this.GetFilesByPostId(id);
                if (file != null)
                {
                    ap = true;
                }
                return ap;


            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;

            }
        }
        public virtual async  Task<bool> BlogHasFiles(string id)
        {
            try
            {

                bool ap = false;
                 
                //   IDataManager.db.Files.FirstOrDefault(x => x.Id == id);
                var file =   this.GetFilesByBlogName(id).Result;
                if (file != null)
                {
                    ap = true;
                }
                return ap;


            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;

            }
        }
        public virtual async  Task<Post> GetPostByFileId(int id)
        {
            try
            {
                Post post= null;

                 
               var tpost =  IDataManager.db.FilesPostsBlog.FirstOrDefault(x => x.FileId == id);
                
                if (   tpost != null)
                {
                    post = await postManager.Details(tpost.PostId);

                }


                return post;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }

        }

        public virtual async  Task<Blog> GetBlofByFileId(int id)
        {
            try
            {
                Blog blog=null;
                
                var file= await this.GetBlofByFileId(id);
                var post = await this.GetPostByFileId(id);
                if ( file!=null &&   post!=null)
                {
                    blog = await this.blogmngr.GetBlogByIdAsync(post.BlogId);


                }


                return blog;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }

        }

       

        

        public Task<Blog> GetBlogByFileId(int id)
        {
            throw new NotImplementedException();
        }
    }
   
}
