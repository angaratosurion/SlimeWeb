using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlimeWeb.Core.Data.ViewModels
{
    public class ViewBlog
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public string Title { get; set; }
        [Display(Name = "Last Updated At")]
        [Required]
        public DateTime LastUpdate { get; set; }
        [Display(Name = "Created  At")]
        [Required]
        public DateTime Created { get; set; }
        public int Administrator { get; set; }
        public string engine { get; set; }
        public virtual List<BlogMods> Moderators { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<Files> Files { get; set; }
        public virtual List<Post> Posts { get; set; }
        public void ImportFromModel(Blog md)
        {
            try
            {
                if (md != null)
                {
                    //ApplicationUser user = CommonTools.Blusrmng.GetUserbyID(md.Author);
                    // if (user != null)
                    {
                        // this.id = md.id;
                        this.Categories = md.Categories;
                        this.content = md.content;
                        this.Published = md.Published;
                        this.Title = md.Title;
                        this.Tags = md.Tags;
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
        public Blog ExportToModel()
        {
            try
            {
                Blog ap = new Blog();

                //// ap.Categories = Categories;
                //ap.Administrator
                //ap.Published = Published;
                //ap.Title = Title;
                //ap.Tags = Tags;
                //ap.Id = Id;
                //this.RowVersion = RowVersion;
                //if (Author != null)
                //{
                //    ap.Author = Author.Id;
                //}

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
