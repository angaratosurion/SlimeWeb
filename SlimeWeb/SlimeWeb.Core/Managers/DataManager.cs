using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
   public  class DataManager
    {
       public static  SlimeDbContext db= new SlimeDbContext();
        public DataManager()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }
        public  async Task<int> PredictLastId(string tablename)
        {
            try
            {
                int ap = -1;
                string sql = String.Format(@"SELECT IDENT_CURRENT ({0}) AS Current_Identity;" + "," + "HMDAdvertiseManage",tablename);
                if (CommonTools.isEmpty(tablename) == false)
                {
                   //await  db.Database.ExecuteSqlRawAsync(sql));
                }





                return ap;



            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return -1;
            }
        }
    }
}
