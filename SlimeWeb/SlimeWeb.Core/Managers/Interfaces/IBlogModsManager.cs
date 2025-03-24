using SlimeWeb.Core.Data.Models;

namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IBlogModsManager
    {
        Task<List<BlogMods>> ListMods();
        Task<BlogMods> Details(string blogname, string modname);
        Task<List<BlogMods>> ListModsByBlogName(string blogname);
        Task<BlogMods> ListModByBlogId(int id);
        Task<List<BlogMods>> ListModsByModName(string modname);
        void RegisterMods(string blogname, string modname);
        void UnRegisterMods(string blogname, string modname);
        Task<BlogMods> Edit(string modname, BlogMods mods, string blogname);
    }


}
