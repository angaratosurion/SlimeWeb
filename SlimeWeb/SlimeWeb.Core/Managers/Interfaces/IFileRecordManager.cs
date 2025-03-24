using SlimeWeb.Core.Data.Models;

namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IFileRecordManager<TBlog, TPost >
    {
        Task<List<Files>> GetFiles();
        Task<string> CreateForBlog(int? blogId, int? postId, 
            Files fileModel, IFormFile fileData, string user);
        Task<string> CreateForPage(string pageName, Files fileModel,
            IFormFile fileData, string user);
        Task<string> CreateForPage(int pageId, Files fileModel,
            IFormFile fileData, string user);
        Task<List<Files>> GetFilesByBlogName(string blogName);
        Task<List<Files>> GetFilesByBlogId(int id);
        Task<List<Files>> GetFilesByPostId(int postId);
        Task<Files> Details(int id);
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
