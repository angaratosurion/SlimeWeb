using SlimeWeb.Core.Data.Models.Interfaces;
using SlimeWeb.Core.Managers.Interfaces;

namespace SlimeWeb.Core.Managers.Managment
{
    public class GroupedManagers
    {
        //public List<IBlogManager> Blogmanagers { get; set; }
          public IPostManager<IPost> PostManager {  get; set; }
        public ICategoryManager<ICategory>  CategoryManager { get; set; }
        public IFileRecordManager<IFiles, IBlog, IFiles>  FileManager  { get; set; }
        public ISlimeWebPageManager<ISlimeWebPage>  PageManager { get; set; }
        public IAccessManager  AccessManager  { get; set; }

    }
}
