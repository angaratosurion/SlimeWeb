using ExtCore.Data.EntityFramework;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeWeb.Core.Data.Repository
{
    public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        SlimeDbContext slimeDb = new SlimeDbContext();
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

        public void EditBasicInfo(Blog bl, string blogname)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        public Blog Get(string Blog)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetAdministrator(string Blogname)
        {
            throw new NotImplementedException();
        }

            public List<Blog> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Blog> GetAllByAdminUser(string username)
        {
            throw new NotImplementedException();
        }

        public List<Blog> GetAllByModUser(string username)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> GetModerators(string Blogname)
        {
            throw new NotImplementedException();
        }
    }
}
