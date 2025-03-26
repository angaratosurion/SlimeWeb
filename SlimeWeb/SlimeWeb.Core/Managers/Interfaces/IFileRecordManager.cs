using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.Models.Interfaces;

namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IFileRecordManager<TBlog, TPost ,TFiles>
    {
        Task<List<TFiles>> GetFiles();
        Task<string> CreateForBlog(int? blogId, int? postId, 
            TFiles fileModel, IFormFile fileData, string user);
        Task<string> CreateForPage(string pageName, TFiles fileModel,
            IFormFile fileData, string user);
        Task<string> CreateForPage(int pageId, TFiles fileModel,
            IFormFile fileData, string user);
        Task<List<TFiles>> GetFilesByBlogName(string blogName);
        Task<List<TFiles>> GetFilesByBlogId(int id);
        Task<List<TFiles>> GetFilesByPostId(int postId);
        Task<TFiles> Details(int id);
        Task<bool> Delete(int id);
        Task<bool> DeleteFromPosts(int id);
        Task<bool> DeleteFromPages(int id);
        Task<bool> DeleteByPostId(int postId);
        void DeleteByBlog(string blogName);
        Task<bool> PostHasFiles(int id);
        Task<bool> BlogHasFiles(string blogName);
        Task<TPost> GetPostByFileId(int id);
        Task<TBlog> GetBlogByFileId(int id);
    }
}
