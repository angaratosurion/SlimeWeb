using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using System.Runtime.CompilerServices;

namespace SlimeWeb.Core.Managers.Interfaces
{

    [Obsolete]
    public interface IBlogManager
    {
        Task<List<T>> ListBlog<T>();
        Task<List<T>> ListBlogBylastUpdated<T>();
        Task<List<T>> ListBlogByAdmUser<T>(string username);
        Task<T?> GetBlog<T>(string name);
        Task<T?> GetBlogById<T>(int? id);
        Task<bool> BlogExists(string name);
        void CreateBlog<T>(T bl, string username);
        Task<T?> EditBasicInfo<T>(T bl, string Blogname);
        Task<List<ApplicationUser>> GetBlogModerators(string Blogname);
        Task MarkAsUpdated(string Blogname, EntityState state);
    }



}
