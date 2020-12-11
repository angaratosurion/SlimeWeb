using ExtCore.Data.Abstractions;
using SlimeWeb.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlimeWeb.Core.Data.Repository.Interface
{
    public interface IBlogRepository:IRepository
    {

        List<Blog> GetAll();
        List<Blog> GetAllByAdminUser(string username);
        List<Blog> GetAllByModUser(string username);
        Blog Get(string Blog);
        Boolean Exists(string name);
        void Create(Blog bl, string username);
        void EditBasicInfo(Blog bl, string blogname);
        List<ApplicationUser> GetModerators(string Blogname);
        ApplicationUser GetAdministrator(string Blogname);
        void Delete(string Blogname);
        void DeleteByAdm(string username);



    }
}
