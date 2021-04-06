using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class CategoryManager:DataManager
    {
        BlogManager blgmng = new BlogManager();
        public async Task<List<Category>>ListCategories()
        {
            try
            {
                return db.Catgories.ToList();
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<Category> GetCategory(string category, string blogname)
        {
            try
            {
                Category cat = null;

                if ((await this.Exists(category, blogname)))
                {
                    List<Category> cats = await  this.ListCategories();
                    Blog blg = await this.blgmng.GetBlogAsync(blogname);
                    
                    if ( cats!=null && blg!=null)
                    {
                        cat = cats.Find(x => x.Name == category && x.BlogId == blg.Id);
                    }

                }

                return cat;
                 
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<Category> GetCategoryById(int cid)
        {
            try
            {
                Category cat = null;

                 
                    List<Category> cats = await this.ListCategories();
                  

                    if (cats != null )
                    {
                    cat = cats.Find(x => x.Id == cid);
                    }

              

                return cat;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<List<Category>> GetCategoryByNameRange(List<string> categorynames,string blogname)
        {
            try
            {
                List<Category> cats = new List<Category>();
                if ( categorynames!=null && CommonTools.isEmpty(blogname))
                {

                    foreach( var catname in categorynames)
                    {
                        var cat = await this.GetCategory(catname, blogname);
                        if ( cat!=null)
                        {
                            cats.Add(cat);
                        }
                    }
                }

                return cats;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

        public async Task<List<Category>> GetCategoryByPostId(int postid)
        {
            try
            {
                List<Category> cats = new List<Category>();
                List<CategotyPost> catpostlst = db.CategoryPosts.ToList();
                if (catpostlst != null)
                {
                    List<CategotyPost> categotyPosts = catpostlst.FindAll(x => x.PostId == postid);
                    if (categotyPosts != null)
                    {
                        foreach(var catp in categotyPosts)
                        {
                            Category cat = await  this.GetCategoryById(catp.CategoryId);
                            if( cat!=null)
                            {
                                cats.Add(cat);
                            }


                        }
                    }
                }



              
                return cats;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<string> GetCategoryNamesToString( string blogname,int bypostid)
        {
            try
            {
                string ap = "" ;


                if (await this.blgmng.BlogExists(blogname))
                {
                    var categories = await this.GetCategoryByPostId(bypostid);
                    if ( categories!=null)
                    {
                        foreach( var cat in categories)
                        {
                            ap+=cat.Name+",";
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


        public async Task<Boolean> Exists(string category,string blogname)

        {
            try
            {
                Boolean ap = false;

                if ((!CommonTools.isEmpty(blogname))&& (!CommonTools.isEmpty(category)))
                {
                    List<Category> cat = await this.ListCategories();
                    List<Blog> blgs =  db.Blogs.ToList();
                    if ((cat.Find(x => x.Name == category) != null)&&(blgs.Find(x=>x.Name==blogname)!=null))
                    {
                        ap = true;
                    }

                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public  async Task AddNew( Category category,string blogname)
        {
            try
            {
                if((category!=null )&& (!CommonTools.isEmpty(blogname) &&((await this.Exists(category.Name,blogname))==false)))
                {
                    db.Catgories.Add(category);
                    await db.SaveChangesAsync();
                }
                    


                    

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        public async void AddNewRange(List<Category> category, string blogname)
        {
            try
            {
                if ((category != null) && (!CommonTools.isEmpty(blogname) ))
                {
                    foreach(var cat in category)
                    {
                       await  this.AddNew(cat, blogname);
                    }
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        public async Task AttachCategorytoPost(string categoryname ,string blogname,int postid)
        {
            try
            {
                if ((!CommonTools.isEmpty(categoryname)&&(!CommonTools.isEmpty(blogname) && ((await this.Exists(categoryname, blogname)) == false))))
                {
                    Category cat =await  this.GetCategory(categoryname, blogname);
                    Blog blg = await this.blgmng.GetBlogAsync(blogname);
                    if (cat != null && blg!=null)
                    {
                        CategotyPost categotyPost = new CategotyPost();

                        categotyPost.BlogId = blg.Id;
                        categotyPost.CategoryId = cat.Id;
                        categotyPost.PostId = postid;
                        db.Add(categotyPost);
                        await db.SaveChangesAsync();
                        


                    }
                    else if(blg!=null)
                    {
                        Category category = new Category();
                        category.BlogId = blg.Id;
                        category.Name = categoryname;
                            
                        await this.AddNew(category, blogname);
                        cat =await  this.GetCategory(categoryname, blogname);
                        if ( cat !=null)
                        {
                            CategotyPost categotyPost = new CategotyPost();

                            categotyPost.BlogId = blg.Id;
                            categotyPost.CategoryId = cat.Id;
                            categotyPost.PostId = postid;
                            db.Add(categotyPost);
                            await db.SaveChangesAsync();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        public async         Task
AttachCategoryRangetoPost(List<string>categoryname, string blogname, int postid)
        {
            try
            {
                if ( (!CommonTools.isEmpty(blogname) && ((categoryname!=null))))
                {
                  
                        foreach(var catname in categoryname)
                        {
                          await AttachCategorytoPost(catname, blogname, postid);
                        }
                   
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }

        
        public async void RemoveCatrgory(string categoryname,string blogname)
        {
            try
            {
                if(CommonTools.isEmpty(categoryname)==false && CommonTools.isEmpty(blogname)==false && await  this.Exists(categoryname,blogname))
                {
                    var cat = await  this.GetCategory(categoryname, blogname);
                    var capost = db.CategoryPosts.ToList().FindAll(x => x.CategoryId == cat.Id);
                    if (cat!=null && capost!=null)
                    {
                         foreach(var c in capost)
                        {
                            db.CategoryPosts.Remove(c);
                         
                            await db.SaveChangesAsync();
                        }
                         db.Catgories.Remove(cat);
                        await db.SaveChangesAsync();

                    }
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }

        } 
        public async Task DetattachCategoryFromPost(int postid,string categoryname,string blogname)
        {
            try
            {

                if (CommonTools.isEmpty(categoryname) == false && CommonTools.isEmpty(blogname) == false && await this.Exists(categoryname, blogname))
                {
                    var cat = await this.GetCategory(categoryname, blogname);
                    var capost = db.CategoryPosts.ToList().FindAll(x => x.CategoryId == cat.Id && x.PostId==postid);
                    if (cat != null && capost != null)
                    {
                        foreach (var c in capost)
                        {
                            db.CategoryPosts.Remove(c);
                            await db.SaveChangesAsync();
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        public async void DettachCategoryRangetoPost(List<string> categoryname, string blogname, int postid)
        {
            try
            {
                if ((!CommonTools.isEmpty(blogname) && ((categoryname != null))))
                {

                    foreach (var catname in categoryname)
                    {
                        await DetattachCategoryFromPost(postid,catname, blogname);
                    }

                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }

    }
}
