using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.ViewModels
{
    public class ViewBlog:Blog
    {

        public void ImportFromModel(Blog md)
        {
            try
            {
                if (md != null )
                {
                    //ApplicationUser user = CommonTools.Blusrmng.GetUserbyID(md.Author);
                    // if (user != null)
                    {

                        this.Id = md.Id;                        
                        
                        this.Created = md.Created;
                     
                        this.Id = md.Id;
                        this.LastUpdate = md.LastUpdate;
                        this.Name = md.Name;
                        this.Title = md.Title;
                        this.Administrator = CommonTools.usrmng.GetUserbyID(md.Administrator);
                         






                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public Blog ExportToModel()
        {
            try
            {
                Blog ap = new Blog();
                ap.Id = this.Id;

                ap.Created = this.Created;
               
                ap.Id = this.Id;
                ap.LastUpdate = this.LastUpdate;
                ap.Name = this.Name;
                ap.Title = this.Title;
               

                    ap.Administrator = this.Administrator.Id;              



                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
            [Required]
        public ApplicationUser Administrator { get; set; }
        public virtual List<BlogMods> Moderators { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<Files> Files { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
}
