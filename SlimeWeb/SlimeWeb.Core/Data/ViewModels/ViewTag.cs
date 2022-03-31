using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.ViewModels
{
    public class ViewTag : Tag
    {

        public async void ImportFromModel(Tag md)
        {
            try
            {
                if (md != null)
                {
                    //ApplicationUser user = CommonTools.Blusrmng.GetUserbyID(md.Author);
                    // if (user != null)
                    {
                        BlogManager blgman = new BlogManager();

                        this.Id = md.Id;

                        this.BlogId = md.BlogId;
                        //this.LastUpdate = md.LastUpdate;
                        this.Name = md.Name;
                        //this.Title = md.Title;
                        Blog blog= (await blgman.GetBlogByIdAsync(this.BlogId)).ExportToModel();
                        this.BlogName = blog.Name;
                        this.BlogTitle = BlogTitle;








                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public Tag ExportToModel()
        {
            try
            {
                Tag ap = new Tag();
                ap.Id = this.Id;



                ap.BlogId = this.BlogId;

                ap.Name = this.Name;




                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        [Required]


       

        //public virtual List<Post> Posts { get; set; }
        public virtual string BlogName { get;set; }
        public virtual string BlogTitle { get; set; }
    }
}

