using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlimeWeb.Core.Data.ViewModels
{
  public   class ViewNews
    {
        [Required]
        public int Id { get; set; }
        //   public int revision { get; set; }
        public string Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Published { get; set; }
        [DataType(DataType.Html)]
        public string content { get; set; }
        public ApplicationUser Author { get; set; }

        public virtual List<Category> Categories { get; set; }
        public virtual List<Tag> Tags { get; set; }
        [ConcurrencyCheck]
        public Byte[] RowVersion { get; set; }


        public void ImportFromModel(News md)
        {
            try
            {
                if (md != null )
                {
                    //ApplicationUser user = CommonTools.Blusrmng.GetUserbyID(md.Author);
                   // if (user != null)
                    {
                        // this.id = md.id;
                        //this.Categories = md.Categories;
                        this.content = md.content;
                        this.Published = md.Published;
                        this.Title = md.Title;
                        //this.Tags = md.Tags;
                        this.Id = md.Id;
                        this.RowVersion = md.RowVersion;
                        //this.Author = user;



                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public News ExportToModel()
        {
            try
            {
                News ap = new News();

                // ap.Categories = Categories;
                ap.content = content;
                ap.Published = Published;
                ap.Title = Title;
                //ap.Tags = Tags;
                ap.Id = Id;
                this.RowVersion = RowVersion;
                if (Author != null)
                {
                    ap.Author = Author.Id;
                }

                //if (Categories == null)
                //{
                //    ap.Categories = new List<Category>();
                //}



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
