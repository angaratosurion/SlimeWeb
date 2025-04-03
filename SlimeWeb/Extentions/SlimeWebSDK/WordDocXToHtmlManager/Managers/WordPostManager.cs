
using DocumentFormat.OpenXml.Bibliography;
using SlimeDockHTml;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;


namespace WordDocXToHtmlManager.Managers
{
    public class WordPostManager:PostManager
    {
        SlimeConverter slimeConverter = new SlimeConverter();
        SlimeWebsUserManager userManager = CommonTools.usrmng;
        WordFileManager wordFileManager = new WordFileManager();
        
        public override Task<Post> Create(Post post, string username)
        {
            try
            {
                Post ap = null;

                if (post != null && CommonTools.isEmpty(username) )
                {
                    ApplicationUser? user = userManager.GetUser(username);
                    if (user != null)
                    {
                        ap = new Post();
                          

                    }

                }
                 
                return Task.FromResult(ap); 

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
        public override async Task<List<Post>> ListByBlogNameByPublished(string name)
        {
            try
            {
                List<Post> ap = null;
                if (CommonTools.isEmpty(name) != true)
                {
                    var files = await wordFileManager.GetFilesByBlogName(name);
                    if (files != null)
                    {
                        ap = new List<Post>();
                        foreach (var file in files)
                        {
                            Post post = new Post();
                            post.Title = file.FileName;
                            post.PostName = file.FileName;
                            post.content = "<a href=\"" + AppSettingsManager.GetPathBase +
                                "/Posts/Details/"+post.PostName+"?bloganame="+name+
                                ">"+ post.Title+"</a>";
                        }

                    }
                }



                return await Task.FromResult(ap);
            }

            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
        public override Task<List<Post>> ListByBlogNameByPublished(string name 
            ,int page, int pagesize)
        {
            try
            {
                return ListByBlogNameByPublished(name);

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
        public override async Task<Post> Details(string postname,string blogname )
        {
            try
            {
                 Post ap=null ,tap= new Post();

                var lstap = await this.List();
                if(lstap != null)
                {
                    tap=lstap.First(x=>x.PostName== postname);
                }

                if ( tap != null)
                {
                    ap=new Post();  
                    ap.Title =tap.Title;
                    ap.PostName=tap.PostName;

                    string filename = Path.Combine(FileSystemManager.GetBlogRootDataFolderAbsolutePath(blogname),
                        postname, ".docx");
                    ap.content=SlimeConverter.ConvertToHtml(filename);

                }

                return await Task.FromResult(ap);

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
        public override async Task<List<Post>> ListByBlogName(string name)
        {
            try
            {
                List<Post> ap = null;
                if (CommonTools.isEmpty(name) != true )
                {
                    var files = await wordFileManager.GetFilesByBlogName(name);
                     
                        if ( files != null )
                        {
                            foreach( var file in files )
                            {
                                var t= new Post();
                                t.Title = Path.GetFileNameWithoutExtension(file.FileName);
                                t.PostName = file.FileName;
                                t.content = "<a href=\"" + AppSettingsManager.GetPathBase +
                                "/Posts/Details/" + t.PostName + "?bloganame=" + name +
                                    ">" + t.Title + "</a>";
                            ap.Add(t);  

                        }
                        }
                     
                }

                return await Task.FromResult(ap);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }

    }
}
