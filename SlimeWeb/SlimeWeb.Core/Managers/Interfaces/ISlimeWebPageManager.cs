namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface ISlimeWebPageManager<T> : IDataManager
    {
        Task<List<T>> List();
        Task<List<T>> ListByPublished();
        Task<List<T>> ListByPublished(int page, int pagesize);
        Task<T> Details(string name);
        Task<T> Details(int id);
        Task<bool> Exists(string name);
        Task<T> Create(T entity, string user);
        Task<T> Edit(string name, T entity);
        Task Delete(string name);
    }

}
