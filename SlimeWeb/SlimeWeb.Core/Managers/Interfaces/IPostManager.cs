namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface IPostManager<T> :IDataManager
    {
        Task<List<T>> List();
        Task<List<T>> ListByPublished();
        Task<List<T>> ListPostByCategory(string categoryName, string blogName);
        Task<List<T>> ListPostByTag(string tagName, string blogName);
        Task<List<T>> ListByBlogName(string name);
        Task<List<T>> ListByBlogNameByPublished(string name);
        Task<List<T>> ListByBlogNameByPublished(string name, int page, int pageSize);
        Task<List<T>> ListAllByPublished();
        Task<List<T>> ListAllByPublished(int page, int pageSize);
        Task<List<T>> ListByBlogId(int? id);
        Task<T> Create(T post, string user);
        Task<T> Details(int? id);
        Task<T> Edit(int? pid, T post);
        Task Delete(int? id);
        Task DeleteByBlogId(int? id);
        bool Exists(int? id);
    }

}
