using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
   public  class DataManager
    {
        public static SlimeDbContext db  = new SlimeDbContext();
                                        // public static AccessManager accessManager = new AccessManager();



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
        //public DataManager(SlimeDbContext slimeDbContext)
        //{
        //    try
        //    {
        //        db = slimeDbContext;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public  async Task<int> PredictLastId(string tablename)
        {
            try
            {
                
                int ap = -1;
                string sql;// String.Format(@"USE {0} Go SELECT IDENT_CURRENT ('{1}') AS Current_Identity;", db.Database.GetDbConnection().Database, tablename);
                if (CommonTools.isEmpty(tablename) == false)
                {
                    sql = String.Format(@"USE [{0}] SELECT IDENT_CURRENT ('{1}') AS Current_Identity;", db.Database.GetDbConnection().Database, tablename);
                    //sql = String.Format(@"SELECT IDENT_CURRENT ('{0}') AS Current_Identity;", tablename);



                    //  db.Database.BeginTransaction();

                    int res = -1;//;= await db.Database.ExecuteSqlRawAsync(sql);
                  var con= db.Database.GetDbConnection();
                    if ( con!=null )
                    {
                        con.Open();
                        var comm=con.CreateCommand();
                        if ( comm !=null)
                        {
                            comm.CommandText = sql;
                            comm.CommandType = System.Data.CommandType.Text;
                            var reader = comm.ExecuteReader();
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    res = Convert.ToInt32(reader["Current_Identity"]);
                                }
                               
                                reader.Close();
                            }
                        }
                        con.Close();
                    }
                  
                   //  db.Database.CommitTransaction();
                   
                    if (res >0)
                    {
                        ap = Convert.ToInt32(res);
                    }
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
