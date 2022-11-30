using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.ViewModels
{
    public class ViewBlogMods : BlogMods
    {
        BlogManager bmngr = new BlogManager( );
        SlimeWebsUserManager userManager = CommonTools.usrmng;
        public string Moderator { get; set; }
        public Blog Blog { get; set; }
        public async void ImportFromModel(BlogMods model)
        {
            if (model != null)
            {
                Blog = await this.bmngr.GetBlogByIdAsync(model.BlogId);
                Moderator =  this.userManager.GetUser(model.ModeratorId).UserName;
                Active= model.Active;
                BlogId = model.BlogId;
                this.ModeratorId = this.userManager.GetUser(model.ModeratorId).UserName;
            }
        }
        public BlogMods ToModel()
        {
            BlogMods ap = new BlogMods();
            ap.BlogId = BlogId;
            ap.ModeratorId = this.userManager.GetUser(ModeratorId).UserName;
            ap.Active = Active;
            
            return ap;
        }
    }
}
