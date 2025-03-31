using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDocXToHtmlManager.Managers
{
    public class WordFileManager:FileRecordManager
    {

        public override Task<List<Files>> GetFilesByBlogName(string BlogName)
        {
			try
			{
                List<Files> ap = null;
                string path=FileManager.GetBlogRootDataFolderAbsolutePath(BlogName);

                return Task.FromResult(ap);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
    }
}
