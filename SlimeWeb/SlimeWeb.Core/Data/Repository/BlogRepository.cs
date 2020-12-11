using ExtCore.Data.EntityFramework;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimeWeb.Core.Data.Repository
{
    public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
      
        public void Create(Blog bl, string username)
        {
            try
            {
                ApplicationUser usr = null;

                if (bl != null && CommonTools.isEmpty(username) == false)
                {



                    usr = CommonTools.usrmng.GetUser(username);
                    if (usr != null)
                    {
                        bl.Administrator = usr.Id;
                        this.dbSet.AddAsync(bl);
                       this.storageContext.SaveChangesAsync();

                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
            }
        }

        public void Delete(string Blogname)
        {
            try
            {

                if (!CommonTools.isEmpty(Blogname))
                {
                    Blog bl = this.Get(Blogname);
                    if( bl!=null)
                    {
                        this.dbSet.Remove(bl);
                       this.storageContext.SaveChangesAsync();

                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }


         public void DeleteByAdm(string username)
        {
            try
            {

                if (!CommonTools.isEmpty(username))
                {

                    List<Blog> bls = this.GetAllByAdminUser(username);
                    if (bls != null)
                    {
                        foreach (Blog w in bls)
                        {
                            this.Delete(w.Name);
                        }
                    }


                }




            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }

        public Blog EditBasicInfo(Blog bl, string Blogname)
        {
            try
            {
                Blog ap = null, bl2;
                if (bl != null && CommonTools.isEmpty(Blogname) == false)
                {



                    bl2 = this.Get(Blogname);
                    bl.Administrator = bl2.Administrator;

                    

                     this.storageContext.Entry(this.Get(Blogname)).CurrentValues.SetValues(bl);
                     this.storageContext.SaveChanges();

                    ap = this.Get(Blogname);
                }


                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

        public bool Exists(string name)
        {
            try
            {
                bool ap = false;
                if (!CommonTools.isEmpty(name))
                {
                    List<Blog> blgs = this.GetAll();
                    if (blgs.Find(x => x.Name == name) != null)
                    {
                        ap = true;
                    }

                }


                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false; ;
            }
        }

        public Blog Get(string name)
        {
            try
            {
                Blog ap = null;
                if (!CommonTools.isEmpty(name))
                {
                    ap = this.GetAll().First(x => x.Name == name);
                    if (ap.Categories == null)
                    {
                        ap.Categories = new List<Category>();
                    }
                    if (ap.Posts == null)
                    {
                        ap.Posts = new List<Post>();
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

        public ApplicationUser GetAdministrator(string Blogname)
        {
            try
            {
                ApplicationUser ap = null;

                if (CommonTools.isEmpty(Blogname) == false && this.Exists(Blogname))
                {
                    Blog bl = this.Get(Blogname);
                    string adm = bl.Administrator;
                    if (CommonTools.isEmpty(adm) == false)
                    {
                        ap = CommonTools.usrmng.GetUserbyID(adm);

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

            public List<Blog> GetAll()
        {
            try
            {
                //return this.db.Blogs.ToList();
                return this.dbSet.ToList<Blog>();
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

        public List<Blog> GetAllByAdminUser(string username)
        {
            try
            {
                List<Blog> ap = null;
                if (!CommonTools.isEmpty(username) && CommonTools.usrmng.UserExists(username))
                {
                    ap = this.GetAll().FindAll(x => x.Administrator == username);
                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

        public List<Blog> GetAllByModUser(string username)
        {
            try
            {
                List<Blog> ap = null;
                if (!CommonTools.isEmpty(username) && CommonTools.usrmng.UserExists(username))
                {
                    //ap = this.ListBlog().FindAll(x => x.Moderators.First(x=>x.Moderator == username));
                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

        public List<ApplicationUser> GetModerators(string Blogname)
        {
            try
            {
                List<ApplicationUser> ap = new List<ApplicationUser>();

                if (CommonTools.isEmpty(Blogname) == false && this.Exists(Blogname))
                {
                    Blog bl = this.Get(Blogname);

                    List<BlogMods> mods = bl.Moderators;
                    if (mods != null)
                    {
                        foreach (var m in mods)
                        {
                            ApplicationUser md = CommonTools.usrmng.GetUserbyID(m.Moderator);
                            if (md != null)
                            {
                                ap.Add(md);
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
    }
}
