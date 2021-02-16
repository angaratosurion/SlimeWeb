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
    }
}
