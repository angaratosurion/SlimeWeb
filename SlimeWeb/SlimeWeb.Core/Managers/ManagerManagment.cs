using SlimeWeb.Core.Data.Models.Interfaces;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;

namespace SlimeWeb.Core.Managers
{
    public static class ManagerManagment
    {
        static List<IBlogManager> Blogmanagers = new List<IBlogManager>();
        static List<IPostManager<IPost>> PostManagers =
            new List<Interfaces.IPostManager<IPost>>();

        public static void RegisterPostManager(  IPostManager<IPost> manager )
        {
            try
            {
                if (manager != null)
                {
                    PostManagers.Add(manager);
                  

                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                
            }
        }
    }
}
