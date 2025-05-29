using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeMarkUpManager.Managers
{
    public class SlimeMarkUpBlogManager:BlogManager
    {
        public override Task<bool> BlogExists(string name)
        {
            try
            {
                bool exists = false;
                var blpath = FileSystemManager.GetBlogRootDataFolderAbsolutePath(name);
                //   if (FileSystemManager.DirectoryExists(blpath) == false)
                {
                  exists=  FileSystemManager.DirectoryExists(blpath);
                    //FileSystemManager.CreateDirectory(blpath);

                }
                return Task.FromResult(exists);
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
                return  Task.FromResult(false);
            }
        }
        public override void CreateBlog(Blog bl, string username)
        {
            try
            {
                if (bl != null )//&& CommonTools.isEmpty(username) == false)
                {
                    string blpath;

                    blpath = FileSystemManager.GetBlogRootDataFolderAbsolutePath(bl.Name);
                   if ((   BlogExists(bl.Name).Result) == false)
                    {
                        FileSystemManager.CreateDirectory(blpath);

                    }
                }

               
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                //return null;
                
            }
        }
    }
}
