using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.ViewModels
{
    public class ViewBlogMods : BlogMods
    {
        BlogManager bmngr = new BlogManager();
        SlimeWebsUserManager userManager = new SlimeWebsUserManager();
        public ApplicationUser Moderator { get; set; }
        public Blog Blog { get; set; }
        public async void ImportFromModel(BlogMods model)
        {
            if (model != null)
            {
                Blog = await this.bmngr.GetBlogByIdAsync(BlogId);
                Moderator =  this.userManager.GetUser(model.ModeratorId);
            }
        }
        public BlogMods ToModel()
        {
            BlogMods ap = new BlogMods();
            ap.BlogId = Blog.Id;
            ap.ModeratorId = Moderator.Id;
            return ap;
        }
    }
}
