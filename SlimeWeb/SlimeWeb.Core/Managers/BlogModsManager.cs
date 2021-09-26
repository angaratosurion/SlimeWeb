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
        public async Task<List<BlogMods>> ListMods(string blogname)
        {
            try
            {
                List<BlogMods> ap = null;
                if(CommonTools.isEmpty(blogname)!=false && (await blmngr.BlogExists(blogname)))
                {
                    Blog blog = await blmngr.GetBlogAsync(blogname);
                    List<BlogMods> tap= DataManager.db.BlogMods.Where(x => x.BlogId == blog.Id).ToList();
                    if( tap!=null)
                    {
                        ap = new List<BlogMods>();
                        foreach(var b in tap)
                        {
                            ViewBlogMods viewBlog = new ViewBlogMods();
                            viewBlog.ImportFromModel(b);
                            ap.Add(viewBlog.ToModel());
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
    }
}
