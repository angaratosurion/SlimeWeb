using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
   public  class PostManager
    {
        SlimeDbContext db;
        BlogManager blmngr;
        public PostManager(SlimeDbContext dbContext)
        {
            db = dbContext;
            blmngr = new BlogManager();
        }

        public async Task<List<Post>> List()
        {
            try
            {
                List<Post> ap = null;

                ap = await db.Post.ToListAsync();
                return ap;

            }
            catch (Exception ex)
            {
                return null;

            }

        }
        public async Task<List<Post>> ListByBlogName(string name)
        {
            try
            {
                List<Post> ap = null, posts;
                Blog blog=null;
                if (name!= null)
                {
                    ap = new List<Post>();
                    if ((await this.blmngr.BlogExists(name)) == true)
                    {
                        blog = await this.blmngr.GetBlogAsync(name);

                        posts = await this.db.Post.Where(x => x.BlogId == blog.Id).ToListAsync();
                        if (posts != null)
                        {
                            ap = posts;
                        }
                    }



                }

                return ap;

            }
            catch (Exception ex)
            {
                return null;

            }

        }
        public async Task<List<Post>> ListByBlogId(int? id)
        {
            try
            {
                List<Post> ap = null, news;
                if (id != null)
                {
                    ap = new List<Post>();
                    news = await this.db.Post.Where(x => x.BlogId == id).ToListAsync();
                    if (news != null)
                    {
                        ap = news;
                    }



                }

                return ap;

            }
            catch (Exception ex)
            {
                return null;

            }

        }
        public async Task<Post> Create(Post Post, string user)
        {
            try
            {
                Post ap = null;

                if (Post != null && user != null)
                {
                    ApplicationUser usr = (ApplicationUser)db.Users.First(m => m.UserName == user);
                    if (usr != null)
                    {
                        Post.Author = usr.UserName;
                       await db.Post.AddAsync(Post);
                        db.SaveChanges();
                        ap = Post;
                    }
                }
                return ap;

            }
            catch (Exception ex) { CommonTools.ErrorReporting(ex); return null; }
        }

        public async Task<Post> Details(int? id)
        {
            try
            {
                Post ap = null;

                if (id != null)
                {
                    ap =await db.Post.FindAsync(id);

                }

                return ap;
            }
            catch (Exception ex) { CommonTools.ErrorReporting(ex); return null; }

        }

        public async Task<Post> Edit(Post Post)
        {
            try
            {
                if (Post != null)
                {
                    db.Entry(Post).State = EntityState.Modified;
                  await  db.SaveChangesAsync();
                }
                return Post;
            }
            catch (Exception ex) { CommonTools.ErrorReporting(ex); return null; }

        }
        public async Task Delete(int? id)
        {
            try
            {
                if (id != null)
                {
                    Post Post =await  db.Post.FindAsync(id);                    
                    db.Post.Remove(Post);
                    db.SaveChanges();
                }

            }
            catch (Exception ex) { CommonTools.ErrorReporting(ex); }

        }
        public async Task  DeleteByBlogId(int? id)
        {
            try
            {
                if (id != null)
                {
                    List<Post> news =await  this.ListByBlogId(id);
                    if (news != null)
                    {
                        foreach (var n in news)
                        {
                           await this.Delete(n.Id);
                        }
                    }
                }

            }
            catch (Exception ex) { CommonTools.ErrorReporting(ex); }

        }
    }
}

