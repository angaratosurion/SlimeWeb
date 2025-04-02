using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Managers.Interfaces;
using System;
using System.IO;

namespace SlimeWeb.Core.Managers
{
    public class JsonExportManager : IExportManager
    {
        public string Name { get{return "Json ExportManager"; } }

      public   void  Export(SlimeDbContext dbContext, string filename)
        {
            try
            {
                if (File.Exists(filename)  && (dbContext!=null))
                {



                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
