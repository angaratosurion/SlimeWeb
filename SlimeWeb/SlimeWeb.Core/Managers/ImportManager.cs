using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Tools;

namespace SlimeWeb.Core.Managers
{
    public static class ImportManager
    {
        static Dictionary<string, IImportManager> ImportManagers =
       new Dictionary<string, IImportManager>();
        public static void Init()
        {

        }
        public static void RegisterMarkupManager(string name,
            IImportManager importManager)
        {
            try
            {

                string ap = null;

                if (CommonTools.isEmpty(name) == false &&
                    importManager != null)
                {
                    ImportManagers.Add(name, importManager);
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }

        }

        public static IImportManager GetImportManager(string importername)
        {
            try
            {
                IImportManager ap = null;
                if (importername != null)
                {
                    ap = ImportManagers.FirstOrDefault(x => x.Key == importername).Value;
                }
                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

                return null;

            }
        }

        public static void Import(string importername, string filename)
        {
            try
            {
                if (importername != null && filename != null)
                {
                    IImportManager importManager = GetImportManager(importername);
                    if (importManager != null)
                    {
                        importManager.Import(filename);

                    }

                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }


    }
}
