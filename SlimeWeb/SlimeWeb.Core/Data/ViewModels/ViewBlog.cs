using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.ViewModels
{
    public class ViewBlog:Blog
    {
        public virtual List<BlogMods> Moderators { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<Files> Files { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
}
