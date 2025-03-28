using Org.BouncyCastle.Bcpg.OpenPgp;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.Models.Interfaces;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;

namespace SlimeWeb.Core.Managers.Managment
{
    public static class ManagerManagment
    {
        static List<IBlogManager> Blogmanagers = new List<IBlogManager>();
        static Dictionary<string,PostManager > PostManagers =
            new Dictionary<string,  PostManager >();
        static Dictionary<string, CategoryManager> CategoryManagers= new 
            Dictionary<string, CategoryManager>();
        static Dictionary<string, IFileRecordManager<IFiles, IBlog,IFiles>> FileManagers =
            new Dictionary<string, IFileRecordManager<IFiles, IBlog, IFiles>>();
        static Dictionary<string, ISlimeWebPageManager<ISlimeWebPage>> PageManagers = 
            new Dictionary<string, ISlimeWebPageManager<ISlimeWebPage>>();
        static Dictionary<string, IAccessManager> AccessManagers =
            new Dictionary<string, IAccessManager>();
        public static void Init()
        {
            PostManager   psotmanager =  new PostManager();
            CategoryManager catmngr = new CategoryManager();
            RegisterPostManager(psotmanager, "SlimePostManager");
            RegisterCategoryManager(catmngr, 
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
        public static GroupedManagers GetDefaultManagger()
        {
            try
            {
                GroupedManagers ap = new GroupedManagers();
                Init();

                ap.AccessManager = GetAccessManagers(AppSettingsManager.GetAccesManager());
                ap.PageManager=GetPageManager(AppSettingsManager.GetDefaultPagesManager());
                ap.FileManager=GetFilesManager(AppSettingsManager.GetDefaultFileManager());
                ap.CategoryManager = GetCategoryManager(AppSettingsManager.
                    GetDefaultCategoryManager());
                ap.PostManager = GetPostManager(AppSettingsManager.GetDefaultPostManager());


                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;

            }


        }

        #region registration for managers
        public static void RegisterPostManager( PostManager manager ,
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
        public static void RegisterCategoryManager(CategoryManager manager,
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

        public static PostManager GetPostManager(string name)
        {
            try
            {
                PostManager  ap = null;
                if (!CommonTools.isEmpty(name) && PostManagers.ContainsKey(name))
                {
                    ap =    PostManagers[name];
                }
                return ap;


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public static CategoryManager GetCategoryManager(string name)
        {
            try
            {
                 CategoryManager ap = null;
                if (!CommonTools.isEmpty(name) && CategoryManagers.ContainsKey(name))
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
                if (!CommonTools.isEmpty(name) && FileManagers.ContainsKey(name))
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
                if (!CommonTools.isEmpty(name) && PageManagers.ContainsKey(name))
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
                if (!CommonTools.isEmpty(name) && AccessManagers.ContainsKey(name))
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
