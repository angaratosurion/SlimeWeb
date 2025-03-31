 
using SlimeDockHTml;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
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
        WordFileManager WordFileManager = new WordFileManager();
        
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
                    var files = await WordFileManager.GetFilesByBlogName(name);
                    if (files != null)
                    {
                        ap = new List<Post>();
                        foreach (var file in files)
                        {
                            Post post = new Post();
                            post.Title = file.FileName;
                            post.content = "<a href=\"" + file.RelativePath + "\""+
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
        public override Task<Post> Details(int? id)
        {
            return base.Details(id);
        }


    }
}
