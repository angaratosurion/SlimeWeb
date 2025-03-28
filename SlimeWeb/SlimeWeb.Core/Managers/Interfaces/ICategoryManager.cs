namespace SlimeWeb.Core.Managers.Interfaces
{
    public interface ICategoryManager<T> : IDataManager
    {
        Task<List<T>> ListCategories();
        Task<List<T>> ListCategoriesByBlog(string blogname);
        Task<T> GetCategory(string category, string blogname);
        Task<T> GetCategoryById(int cid);
        Task<List<T>> GetCategoryByNameRange(List<string> categorynames, string blogname);
        Task<List<T>> GetCategoryByPostId(int postid);
        Task<string> GetCategoryNamesToString(string blogname, int bypostid);
        Task<bool> Exists(string category, string blogname);
        Task<bool> Exists(int categoryid, string blogname);
        Task AddNew(T category, string blogname);
        void AddNewRange(List<T> categories, string blogname);
        void AttachCategoryToPost(string categoryname, string blogname, int postid);
        void AttachCategoryRangeToPost(List<string> categorynames, string blogname, 
            int postid);
        void RemoveCategory(string categoryname, string blogname);
        void RemoveCategory(int categoryId, string blogname);
        void DetattachCategoryFromPost(int postid, string categoryname, string blogname);
        void DettachCategoryRangetoPost(List<string> categorynames, string blogname, 
            int postid);
        Task<T> Edit(int cid, T category, string blogname);
    }

}
