using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Managers.Markups;
using SlimeWeb.Core.Managers.Markups.Interfaces;
using SlimeWeb.Core.Tools;
using System.Runtime.CompilerServices;

namespace SlimeWeb.Core.Managers
{
    public static class ExportManager
    {
        static Dictionary<string, IExportManager> ExportManagers =
            new Dictionary<string, IExportManager>();
        public static void Init()
        {
            JsonExportManager jsonExportManager = new JsonExportManager();
            RegisterMarkupManager(jsonExportManager.Name, jsonExportManager);   
            
        }
        public static void RegisterMarkupManager(string name, 
            IExportManager exportManager)
        {
            try
            {

                string ap = null;

                if (CommonTools.isEmpty(name) == false && exportManager != null)
                {
                    ExportManagers.Add(name, exportManager);
                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }

        }
        public static IExportManager GetExportManager(string exportername)
        {
            try
            {
                IExportManager ap = null;
                if (exportername != null  )
                {
                    ap= ExportManagers.FirstOrDefault(x=>x.Key == exportername).Value;    
                }
                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

                return null;

            }
        }

        public static  void Export(string exportername,string filename)
        {
            try
            {
                if (exportername!= null && filename != null)
                {
                    IExportManager exportManager =  GetExportManager(exportername);
                    if (exportManager != null)
                    {
                        
                        exportManager.Export(  IDataManager.db, filename); 

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
