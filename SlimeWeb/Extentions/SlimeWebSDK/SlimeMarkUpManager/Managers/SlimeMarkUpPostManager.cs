using Microsoft.AspNetCore.Http.HttpResults;
using SlimeMarkUp.Core;
using SlimeMarkUp.Core.Models;
using SlimeMarkUpManager.Managers;
using SlimeMarkUpManager.Managers.MarkupManager;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;


namespace SlimeMarkUp.Managers
{
    public class SlimeMarkUpPostManager:PostManager
    {
         
        SlimeWebsUserManager userManager = CommonTools.usrmng;
        SlimeMarManager slemManrkupManager = new SlimeMarManager();
          SlimeMarkUpFileManager fileManager = new  SlimeMarkUpFileManager();

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
                        post.Author = user.UserName;
                        DocumentProperties documentProperties = new DocumentProperties();
                        documentProperties.Author = user.UserName;
                        documentProperties.Published = post.Published;
                        documentProperties.Title = post.Title;
                        documentProperties.Description = post.Title;
                        documentProperties.Contributors = new List<string> { user.UserName };
                         




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
                    var files = await fileManager.GetFilesByBlogName(name);
                    if (files != null)
                    {
                        ap = new List<Post>();
                        foreach (var file in files)
                        {
                            string input = File.ReadAllText(file.Path);
                            var prop = DocumentPropertiesLoader.Load(file.Path);

                            Post post = new Post();
                            if (prop != null)

                            {
                                post.Author = prop.Author;
                                post.Published = (DateTime)prop.Published;

                                post.Title = prop.Title;
                                post.PostName = file.FileName;
                                post.content = prop.Description;
                            }
                            else
                            {
                                post.Title = file.FileName;
                                post.PostName = file.FileName;
                            }
                            //post.content = "<a href=\"" + AppSettingsManager.GetPathBase +
                            //    "/Posts/Details/" + post.PostName + "?bloganame=" + name +
                            //    ">" + post.Title + "</a>";
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
            , int page, int pagesize)
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

                var lstap = await ListByBlogNameByPublished(blogname);
                if (lstap != null)
                {
                    tap=lstap.First(x=>x.PostName== postname);
                }

                if ( tap != null)
                {


                    ap=new Post();  
                    ap.Title =tap.Title;
                    ap.PostName=tap.PostName;

                    string filename = Path.Combine(FileSystemManager.GetBlogRootDataFolderAbsolutePath(blogname),
                        postname, ".sd");
                  //  ap.content=SlimeConverter.ConvertToHtml(filename);
                 String filecont = File.ReadAllText(filename);
                    if ( CommonTools.isEmpty(filecont)==false)
                    {
                        ap.content =slemManrkupManager.ConvertToHtml(filecont);
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
        //public override async Task<List<Post>> ListByBlogName(string name)
        //{
        //    try
        //    {
        //        List<Post> ap = null;
        //        if (CommonTools.isEmpty(name) != true )
        //        {
        //            var files = await wordFileManager.GetFilesByBlogName(name);
                     
        //                if ( files != null )
        //                {
        //                    foreach( var file in files )
        //                    {
        //                        var t= new Post();
        //                        t.Title = Path.GetFileNameWithoutExtension(file.FileName);
        //                        t.PostName = file.FileName;
        //                        t.content = "<a href=\"" + AppSettingsManager.GetPathBase +
        //                        "/Posts/Details/" + t.PostName + "?bloganame=" + name +
        //                            ">" + t.Title + "</a>";
        //                    ap.Add(t);  

        //                }
        //                }
                     
        //        }

        //        return await Task.FromResult(ap);
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonTools.ErrorReporting(ex);

        //        return null;
        //    }
        //}

    }
}
