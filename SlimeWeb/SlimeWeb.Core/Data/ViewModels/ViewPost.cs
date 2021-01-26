using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.ViewModels
{
   public class ViewPost:Post
    {
        [DataType(DataType.Html)]
        public string HTMLcontent { get; set; }
       
        public ViewPost(Post model)
        {
            this.Author = model.Author;
            this.BlogId = model.BlogId;
            this.Categories = model.Categories;
            this.engine = model.engine;
            this.Id = model.Id;
            this.Published = model.Published;
            this.RowVersion = model.RowVersion;
            this.Tags = model.Tags;
            this.Title = model.Title;
            this.content = model.content;

        }
        public Post ToModel()
        {
            Post ap = new Post();

            ap.Author = Author;
            ap.BlogId = BlogId;
            ap.Categories = Categories;
            ap.engine = engine;
            ap.Id = Id;
            ap.Published = Published;
            ap.RowVersion = RowVersion;
            ap.Tags = Tags;
            ap.Title = Title;
            ap.content = content;


            return ap;
        }
       

    }
}
