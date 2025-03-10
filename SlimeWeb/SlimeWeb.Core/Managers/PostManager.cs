using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
   public  class PostManager:DataManager
    {

        BlogManager blmngr;

        //public PostManager(SlimeDbContext slimeDbContext) : base(slimeDbContext)
        //{
        //    db = slimeDbContext;
        //    blmngr = new BlogManager(slimeDbContext);
        //}
        public PostManager( ) 
        {
            
            blmngr = new BlogManager( );
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
                CommonTools.ErrorReporting(ex);

                return null;
            }

        }
        public async Task<List<Post>> ListByPublished()
        {
            try
            {
                List<Post> ap = null;

                ap = await db.Post.OrderBy(x => x.Published).ToListAsync();
                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }

        }
        public async Task<List<Post>> ListPostByCategory(string categoryname,string blogname)
        {
            try
            {
                List<Post> ap = null;
                CategoryManager categoryManager = new CategoryManager( );

                if( await this.blmngr.BlogExists(blogname) && await categoryManager.Exists(categoryname,blogname ))
                {
                    Category category = await categoryManager.GetCategory(categoryname, blogname);
                    List<CategotyPost> categotyPosts;
                    if(category!=null)
                    {

                        categotyPosts = ( db.CategoryPosts.ToList()).FindAll(x => x.CategoryId == category.Id).ToList();
                        if ( categotyPosts !=null)
                        {
                            ap = new List<Post>();
                            foreach(var catp in categotyPosts)
                            {
                                var post = await this.Details(catp.PostId);
                                ap.Add(post);

                            }
                        }

                    }
                }



                return ap;


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<List<Post>> ListPostByTag(string tagname, string blogname)
        {
            try
            {
                List<Post> ap = null;
                TagManager tagManager = new TagManager( );

                if (await this.blmngr.BlogExists(blogname) && await tagManager.Exists(tagname, blogname))
                {
                   Tag tag = await tagManager.GetTag(tagname, blogname);
                    List<TagPost> tagPosts;
                    if (tag != null)
                    {

                        tagPosts = (db.TagPosts.ToList()).FindAll(x => x.TagId == tag.Id).ToList();
                        if (tagPosts != null)
                        {
                            ap = new List<Post>();
                            foreach (var tagp in tagPosts)
                            {
                                var post = await this.Details(tagp.PostId);
                                ap.Add(post);

                            }
                        }

                    }
                }



                return ap;


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
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

                        posts = await db.Post.Where(x => x.BlogId == blog.Id).ToListAsync();
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
                CommonTools.ErrorReporting(ex);

                return null;
            }

        }
        public async Task<List<Post>> ListByBlogNameByPublished(string name)
        {
            try
            {
                List<Post> ap = null, posts;
                Blog blog = null;
                if (name != null)
                {
                    ap = new List<Post>();
                    if ((await this.blmngr.BlogExists(name)) == true)
                    {
                        blog = await this.blmngr.GetBlogAsync(name);

                        posts = (await this.ListByBlogName(name));
                        posts = posts.OrderByDescending(x => x.Published).ToList();
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
                CommonTools.ErrorReporting(ex);

                return null;
            }

        }
        public async Task<List<Post>> ListByBlogNameByPublished(string name,
            int page, int pagesize)
        {
            try
            {
                List<Post> ap = null, posts=null,tposts;
                Blog blog = null;
                    if(name!=null)
                        {
                         posts = await this.ListByBlogNameByPublished(name);

                    }
                
                if (  pagesize > 0 && page>0 && posts.Count >pagesize)
                {

                   
                    if (posts != null)
                    {
                        ap =  posts.Skip(page * pagesize).Take(pagesize).ToList(); 

                    }


                }
                else //if (pagesize <= 0)
                {
                    ap = await this.ListByBlogNameByPublished(name);
                }
                    return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

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
                    news = await db.Post.Where(x => x.BlogId == id).ToListAsync();
                    if (news != null)
                    {
                        ap = news;
                    }



                }

                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }

        }
        public async Task<Post> Create(Post post, string user)
        {
            try
            {
                Post ap = null;

                if (post != null && user != null)
                {
                    ApplicationUser usr = (ApplicationUser)db.Users.First(m => m.UserName == user);
                    if (usr != null)
                    {
                        post.Author = usr.UserName;
                       await db.Post.AddAsync(post);
                        db.SaveChanges();
                        ap = post;
                        var blog = await this.blmngr.GetBlogByIdAsync(post.BlogId);
                        await this.blmngr.MarkAsUpdated(blog.Name, EntityState.Modified);

                    }
                }
                return ap;

            }
            catch (Exception ex) { 
                CommonTools.ErrorReporting(ex); 
                return null; }
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

        public async Task<Post> Edit(int ? pid,Post post)
        {
            try
            {
                Post vpost = null;
                if (post != null && pid!=null)
                {
                     if (this.Exists(pid))
                    {
                        vpost = await this.Details(pid);
                    }
                    if (vpost != null)
                    {
                         

                        db.Entry(vpost).State = EntityState.Modified;

                        db.Entry(vpost).CurrentValues.SetValues(post);
                        // db.Post.Update(Post);
                        await db.SaveChangesAsync();
                        var blog = await this.blmngr.GetBlogByIdAsync(post.BlogId);
                         
                        await this.blmngr.MarkAsUpdated(blog.Name, EntityState.Modified);
                    }
                }
                return post;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Post)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            // TODO: decide which value should be written to database
                            proposedValues[property] = proposedValue;
                        }

                        // Refresh original values to bypass next concurrency check
                        // entry.OriginalValues.SetValues(databaseValues);
                        entry.OriginalValues.SetValues(proposedValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
                return null;

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
                    FileRecordManager fileRecordManager = new FileRecordManager( );

                    bool deleted=await fileRecordManager.DeleteByPostId((int)id);
                    bool posthasfiles = await fileRecordManager.PostHasFiles((int)id);
                    if (deleted && posthasfiles)
                    {
                        db.Post.Remove(Post);
                        db.SaveChanges();
                        await this.blmngr.MarkAsUpdated((await this.blmngr.GetBlogByIdAsync(Post.Id)).Name, EntityState.Modified);
                    }
                    else
                    {
                        db.Post.Remove(Post);
                        db.SaveChanges();
                        await this.blmngr.MarkAsUpdated((await this.blmngr.GetBlogByIdAsync(Post.Id)).Name, EntityState.Modified);
                    }
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
        public bool Exists(int ?id)
        {
            try
            {
                return db.Post.Any(e => e.Id == id);
            }
            catch (Exception ex )
            {
                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
    }

}

