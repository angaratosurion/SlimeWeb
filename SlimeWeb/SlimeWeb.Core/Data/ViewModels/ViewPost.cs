using Microsoft.AspNetCore.Http;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SlimeWeb.Core.Data.ViewModels
{
   public class ViewPost:Post
    {

        BlogManager bmngr = new BlogManager( );
        [DataType(DataType.Html)]
        public string HTMLcontent { get; set; }
        public Blog Blog { get; set; }
        public List<IFormFile> Files { get; set; }
        public  List<Category> Categories { get; set; }
        public string CategoriesToString { get; set; }
        public string TagsToString { get; set; }
        public List<Tag> Tags { get; set; }
        public ApplicationUser Author { get; set; }
        public void  ImportFromModel(Post model)
        {
            try
            {
                 
                this.Author = IDataManager.db.Users.First(x => x.UserName == model.Author);

                this.BlogId = model.BlogId;
                // this.Categories = model.Categories;

                this.Id = model.Id;
                this.Published = model.Published;
                //this.RowVersion = model.RowVersion;
                //this.Tags = model.Tags;
                this.Title = model.Title;
                this.content = model.content;
                Blog = this.bmngr.GetBlogByIdAsync(model.BlogId).Result.ExportToModel();
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                
            }



        }
        public Post ToModel(string username)
        {
            try
            {
                Post ap = new Post();
                if (CommonTools.isEmpty(username))
                {
                    return null; ;
                }
                if (this.Author == null)
                {
                    Author = IDataManager.db.Users.First(x => x.UserName == username);
                }
                ap.Author = this.Author.UserName;
                ap.BlogId = BlogId;


                ap.Id = Id;
                ap.Published = Published;
               // ap.RowVersion = RowVersion;
                ap.Title = Title;
                ap.content = content;


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
