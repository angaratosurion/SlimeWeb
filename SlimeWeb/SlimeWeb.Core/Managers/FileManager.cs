using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using SlimeWeb.Core;
using SlimeWeb.Core.Managers;

namespace SlimeWeb.Core.Managers
{
    public  class FileManager:FileSystemManager
   {
       CommonTools cmtools = new CommonTools();

        //const string   filesdir="files",AppDataDir="App_Data";
        //static IWebHostEnvironment webHostEnvironment;
        //[DllImport("kernel32.dll")]
        //static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

        //static int SYMLINK_FLAG_DIRECTORY = 1;
        //public FileManager(IWebHostEnvironment twebHostEnvironment)//:base(twebHostEnvironment)
        //{
        //    webHostEnvironment = twebHostEnvironment;
        //}

        // public FileManager (HttpServerUtilityBase tul)
        //{
        //    if ( tul !=null)
        //    {
        //        util = tul;
        //    }
        //}
        #region Common

        public static string PhysicalPathFromUrl(string path)
       {
           try
           {
               string ap = null;
              
               if (path != null && DirectoryExists(path))
               {
                   
                   ap =  Path.Combine(GetAppRootDataFolderAbsolutePath() ,path);
                }
               return ap;

           }
            catch (Exception ex){CommonTools.ErrorReporting(ex);
               return null;
           }
       }
       #endregion
      
    }
}
