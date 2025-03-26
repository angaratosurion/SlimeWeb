using SlimeWeb.Core.Data.Models.Interfaces;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;

namespace SlimeWeb.Core.Managers
{
    public static class ManagerManagment
    {
        static List<IBlogManager> Blogmanagers = new List<IBlogManager>();
        static Dictionary<string,IPostManager<IPost>> PostManagers =
            new Dictionary<string, IPostManager<IPost>>();
        static Dictionary<string, ICategoryManager<ICategory>> CategoryManagers= new 
            Dictionary<string, ICategoryManager<ICategory>>();
        static Dictionary<string, IFileRecordManager<IFiles, IBlog,IFiles>> FileManagers =
            new Dictionary<string, IFileRecordManager<IFiles, IBlog, IFiles>>();
        static Dictionary<string, ISlimeWebPageManager<ISlimeWebPage>> PageManagers = 
            new Dictionary<string, ISlimeWebPageManager<ISlimeWebPage>>();

        #region registration for managers
        public static void RegisterPostManager( IPostManager<IPost> manager ,
            string managername)
        {
            try
            {
                if (manager != null && !CommonTools.isEmpty(managername))
                {
                    PostManagers.Add(managername,manager);

                }
                 
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                
            }
        }
        public static void RegisterCategoryManager(ICategoryManager<ICategory> manager,
           string managername)
        {
            try
            {
                if (manager != null && !CommonTools.isEmpty(managername))
                {
                    CategoryManagers.Add(managername, manager);

                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

            }
        }
        public static void RegisterFilesManager(IFileRecordManager<IFiles, IBlog, IFiles> 
            manager,
          string managername)
        {
            try
            {
                if (manager != null && !CommonTools.isEmpty(managername))
                {
                    FileManagers.Add(managername, manager);

                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

            }
        }
        public static void RegisterPageManager(ISlimeWebPageManager<ISlimeWebPage>
           manager,
         string managername)
        {
            try
            {
                if (manager != null && !CommonTools.isEmpty(managername))
                {
                    PageManagers.Add(managername, manager);

                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

            }
        }


        #endregion
    }
}
