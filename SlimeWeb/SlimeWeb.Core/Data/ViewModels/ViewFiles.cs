using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.NonDataModels;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Data.ViewModels
{
    public class ViewFiles:Files
    {
        public int BlogId { get; set; }
        public int PostId { get; set; }
        FileRecordManager filmngr = new FileRecordManager( );
        public ApplicationUser Owner { get; set; }
        public ExifModel ExifData { get; set; }
        public void ImportFromModel(Files model)
        {
            try
            {
                int blogid, postid;
                this.ContentType = model.ContentType;
                this.FileName = model.FileName;
                this.Id = model.Id;
                this.Owner = BlogManager.db.Users.First(x => x.UserName == model.Owner);
                this.Path = model.Path;
                this.RelativePath = model.RelativePath;
                this.RowVersion = model.RowVersion;


                using (SlimeDbContext db = new SlimeDbContext())
                {

                    FilesPostBlog filesPostBlog = db.FilesPostsBlog.First(x => x.FileId == model.Id);
                    blogid = filesPostBlog.BlogId;
                    postid = filesPostBlog.PostId;

                }
                this.BlogId = blogid;
                this.PostId = postid;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

               
            }

        }
        public Files ToModel(string username)
        {
            try
            {
                Files ap = new Files();
                if (CommonTools.isEmpty(username))
                {
                    return null; ;
                }
                if (this.Owner == null)
                {
                    Owner = BlogManager.db.Users.First(x => x.UserName == username);
                }
                ap.Owner = this.Owner.UserName;
                ap.Id = Id;
                ap.RowVersion = RowVersion;
                ap.ContentType = this.ContentType;
                ap.FileName = this.FileName;
                ap.Path = this.Path;
                ap.RelativePath = this.RelativePath;

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
