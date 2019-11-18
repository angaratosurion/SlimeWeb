using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SlimeWeb.Core
{
    public class CommonTools
    {
       
        public static Boolean isEmpty(string str)
        {
            try
            {
                Boolean ap = true;
                if (str != null && str != String.Empty)
                {
                    ap = false;
                }

                return ap;
            }
            catch (Exception)
            {

                throw;
                return true;
            }
        }
        public static void ErrorReporting(Exception ex)
        {
            //throw (ex);
            //BlackCogs.Configuration.BlackCogsSettingManager conf = new Configuration.BlackCogsSettingManager();
            if (ex.GetBaseException() is ValidationException)
            {
               // ValidationErrorReporting((ValidationException)ex);


            }
            else
            {

                NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Fatal(ex);
               // if (conf.ExceptionShownOnBrowser() == true)
                {

                    throw (ex);
                }
            }

        }
        
    }
}
