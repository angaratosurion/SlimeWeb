using Org.BouncyCastle.Bcpg.OpenPgp;
using SlimeWeb.Core.Data.Models;
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
        static Dictionary<string, IAccessManager> AccessManagers =
            new Dictionary<string, IAccessManager>();
        public static void Init()
        {
            var psotmanager = (IPostManager<Post>)new PostManager();
            CategoryManager catmngr = new CategoryManager();
            RegisterPostManager((IPostManager<IPost>)psotmanager, 
                "SlimePostManager");
            RegisterCategoryManager((ICategoryManager<ICategory>)catmngr, 
                "SlimeCategoryManager");
            FileRecordManager fileRecordManager = new FileRecordManager();
            RegisterFilesManager((IFileRecordManager<IFiles, IBlog, IFiles>)fileRecordManager, 
                "SlimeFileManager");
            AccessManager accessmgr = new AccessManager();
            RegisterAccessManagers( accessmgr, "SlimeAccessManager");
            SlimeWebPageManager slimeWebPageManager = new SlimeWebPageManager();
            RegisterPageManager((ISlimeWebPageManager<ISlimeWebPage>)slimeWebPageManager,
                "SlimeWebPageManager");


        }

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
        public static void RegisterAccessManagers(IAccessManager
          manager,
        string managername)
        {
            try
            {
                if (manager != null && !CommonTools.isEmpty(managername))
                {
                    AccessManagers.Add(managername, manager);

                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

            }
        }


        #endregion
        #region getDeffaultInstances

        public static IPostManager<IPost> GetPostManager(string name)
        {
            try
            {
                IPostManager<IPost> ap = null;
                if (!CommonTools.isEmpty(name) && ((PostManagers.ContainsKey(name))))
                {
                    ap = PostManagers[name];
                }
                return ap;


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public static ICategoryManager<ICategory> GetCategoryManager(string name)
        {
            try
            {
                ICategoryManager<ICategory> ap = null;
                if (!CommonTools.isEmpty(name) && (CategoryManagers.ContainsKey(name)))
                {
                    ap = CategoryManagers[name];
                }
                return ap;


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public static IFileRecordManager<IFiles, IBlog, IFiles> GetFilesManager(string 
            name)
        {
            try
            {
                IFileRecordManager<IFiles, IBlog, IFiles> ap = null;
                if (!CommonTools.isEmpty(name) && ((FileManagers.ContainsKey(name))))
                {
                    ap = FileManagers[name];
                }
                return ap;


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public static ISlimeWebPageManager<ISlimeWebPage> GetPageManager(string name)
        {
            try
            {
                ISlimeWebPageManager < ISlimeWebPage > ap = null;
                if (!CommonTools.isEmpty(name) && ((PageManagers.ContainsKey(name))))
                {
                    ap = PageManagers[name];
                }
                return ap;


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public static IAccessManager GetAccessManagers(string name)
        {
            try
            {
                IAccessManager ap = null;
                if (!CommonTools.isEmpty(name) && ((AccessManagers.ContainsKey(name))))
                {
                    ap = AccessManagers[name];
                }
                return ap;


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;

            }
        }





        #endregion
    }
}
