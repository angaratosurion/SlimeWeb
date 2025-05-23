﻿using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class CategoryManager : IDataManager
    {
        BlogManager blgmng = new BlogManager();

        public virtual async  Task<List<Category>> ListCategories()
        {
            try
            {
                return  IDataManager.db.Catgories.ToList();
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public virtual async  Task<List<Category>> ListCategoriesByBlog(string blogname)
        {
            try
            {
                List<Category> ap = null;
                var blog = await this.blgmng.GetBlogAsync(blogname);
                if (blog != null)
                {
                    ap = new List<Category>();
                    var categories = await this.ListCategories();
                    ap = categories.FindAll(x => x.BlogId == blog.Id).ToList();

                }

                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public virtual async  Task<Category> GetCategory(string category, string blogname)
        {
            try
            {
                Category cat = null;

                if ((await this.Exists(category, blogname)))
                {
                    List<Category> cats = await this.ListCategories();
                    Blog blg = await this.blgmng.GetBlogAsync(blogname);

                    if (cats != null && blg != null)
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
        public virtual async  Task<Category> GetCategoryById(int cid)
        {
            try
            {
                Category cat = null;


                List<Category> cats = await this.ListCategories();


                if (cats != null)
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
        public virtual async  Task<List<Category>> GetCategoryByNameRange(List<string> categorynames, string blogname)
        {
            try
            {
                List<Category> cats = new List<Category>();
                if (categorynames != null && CommonTools.isEmpty(blogname))
                {

                    foreach (var catname in categorynames)
                    {
                        var cat = await this.GetCategory(catname, blogname);
                        if (cat != null)
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

        public virtual async  Task<List<Category>> GetCategoryByPostId(int postid)
        {
            try
            {
                List<Category> cats = new List<Category>();
                List<CategotyPost> catpostlst =  IDataManager.db.CategoryPosts.ToList();
                if (catpostlst != null)
                {
                    List<CategotyPost> categotyPosts = catpostlst.FindAll(x => x.PostId == postid);
                    if (categotyPosts != null)
                    {
                        foreach (var catp in categotyPosts)
                        {
                            Category cat = await this.GetCategoryById(catp.CategoryId);
                            if (cat != null)
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
        public virtual async  Task<string> GetCategoryNamesToString(string blogname, int bypostid)
        {
            try
            {
                string ap = "";


                if (await this.blgmng.BlogExists(blogname))
                {
                    var categories = await this.GetCategoryByPostId(bypostid);
                    if (categories != null)
                    {
                        foreach (var cat in categories)
                        {
                            ap += cat.Name + ",";
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


        public virtual async  Task<Boolean> Exists(string category, string blogname)

        {
            try
            {
                Boolean ap = false;

                if ((!CommonTools.isEmpty(blogname)) && (!CommonTools.isEmpty(category)))
                {
                    List<Category> cat = await this.ListCategories();
                    List<Blog> blgs =  IDataManager.db.Blogs.ToList();
                    if ((cat != null && cat.Find(x => x.Name == category) != null) && (blgs.Find(x => x.Name == blogname) != null))
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
        public virtual async  Task<Boolean> Exists(int categoryid, string blogname)

        {
            try
            {
                Boolean ap = false;

                if ((!CommonTools.isEmpty(blogname)))
                {
                    List<Category> cat = await this.ListCategories();
                    List<Blog> blgs =  IDataManager.db.Blogs.ToList();
                    if ((cat.Find(x => x.Id == categoryid) != null) &&
                        (blgs.Find(x => x.Name == blogname) != null))
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
        public virtual async  Task AddNew(Category category, string blogname)
        {
            try
            {
                if ((category != null) && (!CommonTools.isEmpty(blogname) && ((await this.Exists(category.Name, blogname)) == false)))
                {

                    if (CommonTools.isEmpty(category.BlogAndCategory) || !(category.BlogAndCategory.StartsWith(blogname) ||
                        !(category.BlogAndCategory.EndsWith(category.Name))))
                    {
                        category.BlogAndCategory = blogname + "_" + category.Name;
                    }
                     IDataManager.db.Catgories.Add(category);
                    await  IDataManager.db.SaveChangesAsync();
                    await this.blgmng.MarkAsUpdated(blogname, EntityState.Modified);

                }





            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        public virtual async  void AddNewRange(List<Category> category, string blogname)
        {
            try
            {
                if ((category != null) && (!CommonTools.isEmpty(blogname)))
                {
                    foreach (var cat in category)
                    {
                        await this.AddNew(cat, blogname);
                    }
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        public void AttachCategorytoPost(string categoryname, string blogname,
            int postid)
        {
            try
            {
                if ((!CommonTools.isEmpty(categoryname) && (!CommonTools.isEmpty(blogname) && 
                    ((  Exists(categoryname, blogname).Result) == false))))
                {

                    Blog blg =   blgmng.GetBlogAsync(blogname).Result ;
                    if (blg != null)
                    {
                        Category cat = new Category();
                        cat.BlogId = blg.Id;
                        cat.Name = categoryname;

                          this.AddNew(cat, blogname);
                        cat =   this.GetCategory(categoryname, blogname).Result;
                        if (cat != null)
                        {
                            CategotyPost categotyPost = new CategotyPost();

                            categotyPost.BlogId = blg.Id;
                            categotyPost.CategoryId = cat.Id;
                            categotyPost.PostId = postid;
                             IDataManager.db.Add(categotyPost);
                              IDataManager.db.SaveChangesAsync();
                              blgmng.MarkAsUpdated(blogname, EntityState.Modified);
                        }
                    }

                }
                else if ((!CommonTools.isEmpty(categoryname) && (!CommonTools.isEmpty(blogname) && 
                    ((Exists(categoryname, blogname).Result)))))
                {

                    Category cat =  GetCategory(categoryname, blogname).Result;
                    Blog blg = this.blgmng.GetBlogAsync(blogname).Result;
                    if (cat != null && blg != null)
                    {
                        CategotyPost categotyPost = new CategotyPost();

                        categotyPost.BlogId = blg.Id;
                        categotyPost.CategoryId = cat.Id;
                        categotyPost.PostId = postid;
                         IDataManager.db.Add(categotyPost);
                         IDataManager.db.SaveChangesAsync();
                         blgmng.MarkAsUpdated(blogname, EntityState.Modified);



                    }



                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        public void AttachCategoryRangeToPost(List<string> categoryname,
            string blogname, int postid)
        {
            try
            {
                if ((!CommonTools.isEmpty(blogname) && ((categoryname != null))))
                {

                    foreach (var catname in categoryname)
                    {
                        AttachCategorytoPost(catname, blogname, postid);
                    }

                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }


        public virtual async  void RemoveCategory(string categoryname, string blogname)
        {
            try
            {
                if (CommonTools.isEmpty(categoryname) == false && 
                    CommonTools.isEmpty(blogname) == false && await this.Exists(categoryname, blogname))
                {
                    var cat = await this.GetCategory(categoryname, blogname);
                    var capost =  IDataManager.db.CategoryPosts.ToList().FindAll(x => x.CategoryId == cat.Id);
                    if (cat != null && capost != null)
                    {
                        foreach (var c in capost)
                        {
                             IDataManager.db.CategoryPosts.Remove(c);

                            await  IDataManager.db.SaveChangesAsync();
                        }
                         IDataManager.db.Catgories.Remove(cat);
                        await  IDataManager.db.SaveChangesAsync();
                        await this.blgmng.MarkAsUpdated(blogname, EntityState.Modified);

                    }
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }

        }
        public virtual async  void RemoveCategory(int categoryname, string blogname)
        {
            try
            {
                if (CommonTools.isEmpty(blogname) == false && await this.Exists(categoryname, blogname))
                {
                    var cat = await this.GetCategoryById(categoryname);
                    var capost =  IDataManager.db.CategoryPosts.ToList().FindAll(x => x.CategoryId == cat.Id);
                    if (cat != null && capost != null)
                    {
                        foreach (var c in capost)
                        {
                             IDataManager.db.CategoryPosts.Remove(c);

                            await  IDataManager.db.SaveChangesAsync();
                        }
                         IDataManager.db.Catgories.Remove(cat);
                        await  IDataManager.db.SaveChangesAsync();
                        await this.blgmng.MarkAsUpdated(blogname, EntityState.Modified);

                    }
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }

        }
        public void DetattachCategoryFromPost(int postid, string categoryname,
            string blogname)
        {
            try
            {

                if (CommonTools.isEmpty(categoryname) == false && 
                    CommonTools.isEmpty(blogname) == false &&
                     Exists(categoryname, blogname).Result)
                {
                    var cat = GetCategory(categoryname, blogname).Result;
                    var capost =  IDataManager.db.CategoryPosts.ToList().FindAll(x => x.CategoryId == cat.Id && x.PostId == postid);
                    if (cat != null && capost != null)
                    {
                        foreach (var c in capost)
                        {
                             IDataManager.db.CategoryPosts.Remove(c);
                               IDataManager.db.SaveChangesAsync();
                        }
                          this.blgmng.MarkAsUpdated(blogname, EntityState.Modified);

                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        public void DettachCategoryRangetoPost(List<string> categoryname,
            string blogname, int postid)
        {
            try
            {
                if ((!CommonTools.isEmpty(blogname) && ((categoryname != null))))
                {

                    foreach (var catname in categoryname)
                    {
                        DetattachCategoryFromPost(postid, catname, blogname);
                    }

                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        public virtual async  Task<Category> Edit(int cid, Category category, string blogname)
        {
            try
            {
                Category vcategory = null;
                if (category != null && !CommonTools.isEmpty(blogname))
                {
                    if (await this.Exists(cid, blogname))
                    {
                        vcategory = await this.GetCategoryById(cid);
                    }
                    if (vcategory != null)
                    {
                        category.BlogId = vcategory.BlogId;

                         IDataManager.db.Entry(vcategory).State = EntityState.Modified;

                         IDataManager.db.Entry(vcategory).CurrentValues.SetValues(category);
                        //  IDataManager.db.Post.Update(Post);
                        await  IDataManager.db.SaveChangesAsync();
                        await this.blgmng.MarkAsUpdated(blogname, EntityState.Modified);
                    }
                }
                return category;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Category)
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
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;


            }

        }

        public void AttachCategoryToPost(string categoryname, string blogname, int postid)
        {
            AttachCategorytoPost(categoryname, blogname, postid);
        }
    }
}



