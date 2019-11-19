using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BlackOwl.Core.Managers
{
   public class PluginManager
    {
       const string   filesdir="files",AppDataDir="App_Data";
       HttpServerUtilityBase util;
       FileManager filmngr;
       //public PluginManager(HttpServerUtilityBase tul)
       //{
       //    if ( tul !=null)
       //    {
       //        util = tul;
       //        filmngr = new FileManager(tul);
       //    }
       //}

       public string GetFilesPthysicalDir()
       {
           try
           {
               string ap = null;
               string path = FileManager.PhysicalPathFromUrl("~/" + AppDataDir + "/" + filesdir);

               if (path != null && FileManager.DirectoryExists(path))
               {
                   ap = path;
               }

               return ap;

           }
           catch (Exception ex){CommonTools.ErrorReporting(ex);
               return null;
               
           }
       }
       public string GetPluginFilesPthysicalDir(string pluginname)
       {
           try
           {
               string ap = null;
               if (pluginname != null)
               {
                   string path = FileManager.PhysicalPathFromUrl("~/" + AppDataDir + "/" + filesdir + "/" + pluginname);

                   if (path != null && FileManager.DirectoryExists(path))
                   {
                       ap = path;
                   }
               }

               return ap;

           }
           catch (Exception ex){CommonTools.ErrorReporting(ex);
               return null;
               
           }
       }
        public string GetPluginFilesRelativeDir(string pluginname)
        {
            try
            {
                string ap = null;
                if (pluginname != null)
                {
                    string path = "~/" + AppDataDir + "/" + filesdir + "/" + pluginname;

                    if (path != null && FileManager.DirectoryExists(path))
                    {
                        ap = path;
                    }
                }

                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;

            }
        }

    }
}
