using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public class TagManager:DataManager
    {
        BlogManager blgmng = new BlogManager();
        public async Task<List<Tag>>ListTags()
        {
            try
            {
                return db.Tags.ToList();
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<Tag> GetTag(string Tag, string blogname)
        {
            try
            {
                Tag cat = null;

                if ((await this.Exists(Tag, blogname)))
                {
                    List<Tag> cats = await  this.ListTags();
                    Blog blg = await this.blgmng.GetBlogAsync(blogname);
                    
                    if ( cats!=null && blg!=null)
                    {
                        cat = cats.Find(x => x.Name == Tag && x.BlogId == blg.Id);
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
        public async Task<Tag> GetTagById(int cid)
        {
            try
            {
                Tag cat = null;

                 
                    List<Tag> cats = await this.ListTags();
                  

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
        public async Task<List<Tag>> GetTagByNameRange(List<string> Tagnames,string blogname)
        {
            try
            {
                List<Tag> cats = new List<Tag>();
                if ( Tagnames!=null && CommonTools.isEmpty(blogname))
                {

                    foreach( var catname in Tagnames)
                    {
                        var cat = await this.GetTag(catname, blogname);
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

        public async Task<List<Tag>> GetTagByPostId(int postid)
        {
            try
            {
                List<Tag> cats = new List<Tag>();
                List<TagPost> catpostlst = db.TagPosts.ToList();
                if (catpostlst != null)
                {
                    List<TagPost> TagPosts = catpostlst.FindAll(x => x.PostId == postid);
                    if (TagPosts != null)
                    {
                        foreach(var catp in TagPosts)
                        {
                            Tag cat = await  this.GetTagById(catp.TagId);
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
        public async Task<string> GetTagNamesToString( string blogname,int bypostid)
        {
            try
            {
                string ap = "" ;


                if (await this.blgmng.BlogExists(blogname))
                {
                    var Tags = await this.GetTagByPostId(bypostid);
                    if ( Tags!=null)
                    {
                        foreach( var cat in Tags)
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


        public async Task<Boolean> Exists(string Tag,string blogname)

        {
            try
            {
                Boolean ap = false;

                if ((!CommonTools.isEmpty(blogname))&& (!CommonTools.isEmpty(Tag)))
                {
                    List<Tag> cat = await this.ListTags();
                    List<Blog> blgs =  db.Blogs.ToList();
                    if ((cat.Find(x => x.Name == Tag) != null)&&(blgs.Find(x=>x.Name==blogname)!=null))
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
        public  async Task AddNew( Tag Tag,string blogname)
        {
            try
            {
                if((Tag!=null )&& (!CommonTools.isEmpty(blogname) &&((await this.Exists(Tag.Name,blogname))==false)))
                {
                    db.Tags.Add(Tag);
                    await db.SaveChangesAsync();
                }
                    


                    

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }
        public async void AddNewRange(List<Tag> Tag, string blogname)
        {
            try
            {
                if ((Tag != null) && (!CommonTools.isEmpty(blogname) ))
                {
                    foreach(var cat in Tag)
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
        public async Task AttachTagtoPost(string Tagname ,string blogname,int postid)
        {
            try
            {
                if ((!CommonTools.isEmpty(Tagname)&&(!CommonTools.isEmpty(blogname) && ((await this.Exists(Tagname, blogname)) == false))))
                {
                    Tag cat =await  this.GetTag(Tagname, blogname);
                    Blog blg = await this.blgmng.GetBlogAsync(blogname);
                    if (cat != null && blg!=null)
                    {
                        TagPost TagPost = new TagPost();

                        TagPost.BlogId = blg.Id;
                        TagPost.TagId = cat.Id;
                        TagPost.PostId = postid;
                        db.Add(TagPost);
                        await db.SaveChangesAsync();
                        


                    }
                    else if(blg!=null)
                    {
                        Tag Tag = new Tag();
                        Tag.BlogId = blg.Id;
                        Tag.Name = Tagname;
                            
                        await this.AddNew(Tag, blogname);
                        cat =await  this.GetTag(Tagname, blogname);
                        if ( cat !=null)
                        {
                            TagPost TagPost = new TagPost();

                            TagPost.BlogId = blg.Id;
                            TagPost.TagId = cat.Id;
                            TagPost.PostId = postid;
                            db.Add(TagPost);
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
        public async Task AttachTagRangetoPost(List<string>Tagname, string blogname, int postid)
        {
            try
            {
                if ( (!CommonTools.isEmpty(blogname) && ((Tagname!=null))))
                {
                  
                        foreach(var catname in Tagname)
                        {
                          await AttachTagtoPost(catname, blogname, postid);
                        }
                   
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }

        
        public async void RemoveCatrgory(string Tagname,string blogname)
        {
            try
            {
                if(CommonTools.isEmpty(Tagname)==false && CommonTools.isEmpty(blogname)==false && await  this.Exists(Tagname,blogname))
                {
                    var cat = await  this.GetTag(Tagname, blogname);
                    var capost = db.TagPosts.ToList().FindAll(x => x.TagId == cat.Id);
                    if (cat!=null && capost!=null)
                    {
                         foreach(var c in capost)
                        {
                            db.TagPosts.Remove(c);
                         
                            await db.SaveChangesAsync();
                        }
                         db.Tags.Remove(cat);
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
        public async Task DetattachTagFromPost(int postid,string Tagname,string blogname)
        {
            try
            {

                if (CommonTools.isEmpty(Tagname) == false && CommonTools.isEmpty(blogname) == false && await this.Exists(Tagname, blogname))
                {
                    var cat = await this.GetTag(Tagname, blogname);
                    var capost = db.TagPosts.ToList().FindAll(x => x.TagId == cat.Id && x.PostId==postid);
                    if (cat != null && capost != null)
                    {
                        foreach (var c in capost)
                        {
                            db.TagPosts.Remove(c);
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
        public async void DettachTagRangetoPost(List<string> Tagname, string blogname, int postid)
        {
            try
            {
                if ((!CommonTools.isEmpty(blogname) && ((Tagname != null))))
                {

                    foreach (var catname in Tagname)
                    {
                        await DetattachTagFromPost(postid,catname, blogname);
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
