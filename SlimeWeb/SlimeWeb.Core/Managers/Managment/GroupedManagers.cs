using SlimeWeb.Core.Data.Models.Interfaces;
using SlimeWeb.Core.Managers.Interfaces;

namespace SlimeWeb.Core.Managers.Managment
{
    public class GroupedManagers
    {
        //public List<IBlogManager> Blogmanagers { get; set; }
          public PostManager PostManager {  get; set; }
        public CategoryManager  CategoryManager { get; set; }
        public FileRecordManager  FileManager  { get; set; }
        public SlimeWebPageManager PageManager { get; set; }
        public AccessManager  AccessManager  { get; set; }

    }
}
