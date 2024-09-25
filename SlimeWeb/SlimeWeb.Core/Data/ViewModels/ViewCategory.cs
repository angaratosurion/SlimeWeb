using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.ViewModels
{
    public class ViewCategory : Category
    {

        public async void ImportFromModel(Category md)
        {
            try
            {
                if (md != null)
                {
                    //ApplicationUser user = CommonTools.Blusrmng.GetUserbyID(md.Author);
                    // if (user != null)
                    {
                        BlogManager blgman = new BlogManager( );

                        this.Id = md.Id;

                        this.BlogId = md.BlogId;
                        //this.LastUpdate = md.LastUpdate;
                        this.Name = md.Name;
                        //this.Title = md.Title;
                        Blog blog= (await blgman.GetBlogByIdAsync(this.BlogId)).ExportToModel();
                        this.BlogName = blog.Name;
                        this.BlogTitle = BlogTitle;
                        this.BlogAndCategory = BlogName+"_"+Name;








                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public Category ExportToModel()
        {
            try
            {
                Category ap = new Category();
                ap.Id = this.Id;



                ap.BlogId = this.BlogId;

                ap.Name = this.Name;
                ap.BlogAndCategory = this.BlogAndCategory;
                ap.BlogId= this.BlogId;
                //ap.RowVersion = this.RowVersion;




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

