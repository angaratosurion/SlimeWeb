using Microsoft.AspNetCore.Http;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class FileRecordManager : DataManager
    {
        BlogManager blogmngr = new BlogManager();


        PostManager postManager = new PostManager();
        //public FileRecordManager(SlimeDbContext tdb)
        //{
        //    db = tdb;
        //    postManager = new PostManager(db);
        //}
        public async Task<string> Create (int BlogId,int postid,Files filemodel, FormFile filedata)
        {
            try
            { string ap = null;
                if((filedata!=null) && (filemodel!=null))
                {
                    var blog = await blogmngr.GetBlogByIdAsync(BlogId);
                    var post = await postManager.Details(postid);
                    if ( (blog !=null) && (post!=null))
                    {
                        var blogpath = FileSystemManager.GetBlogRootDataFolderAbsolutePath(blog.Name);
                       string  abspath=await  FileSystemManager.CreateFile(blogpath, filedata);
                        if (!CommonTools.isEmpty(abspath))
                        {
                            ap = FileSystemManager.GetBlogRootDataFolderRelativePath(blog.Name) + "\\" + Path.GetFileName(abspath);
                            filemodel.FileName = Path.GetFileName(abspath);
                            filemodel.Path = abspath;
                            filemodel.RelativePath = ap;
                            db.Files.Add(filemodel);
                           await  db.SaveChangesAsync();
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
        public async  Task<List<Files>> GetFilesByBlogName(string BlogName)
        {
            try
            {
                List<Files> ap = new List<Files>();
                if ( (!CommonTools.isEmpty(BlogName) )   &&(await blogmngr.BlogExists(BlogName)))
                {
                    var blog = await blogmngr.GetBlogAsync(BlogName);
                    
                    if( blog !=null )
                    {
                        ap =  db.Files.Where(x => x.BlogId == blog.Id).ToList();
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
        public async Task<List<Files>> GetFilesByBlogId(int id)
        {
            try
            {
                List<Files> ap = new List<Files>();

                var blog = await blogmngr.GetBlogByIdAsync(id);

                    if (blog != null)
                    {
                        ap = db.Files.Where(x => x.BlogId == blog.Id).ToList();
                    }

                
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<Files> Details(int id)
        {
            try
            {
                Files ap = null;

                ap = db.Files.FirstOrDefault(x => x.Id == id);

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async void Delete(int id)
        {
            try
            {
                 

                db.Files.FirstOrDefault(x => x.Id == id);
                var file = await this.Details(id);
                if (file!=null)
                {
                    FileSystemManager.DeleteFile(file.RelativePath);
                    db.Files.Remove(file);
                }

                 

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
               
            }
        }
        public async void DeleteByBlog(string blogname)
        {
            try
            {



                var files = await this.GetFilesByBlogName(blogname);
                if (files != null)
                {
                    foreach(var file in files)
                    {
                        this.Delete(file.Id);
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
