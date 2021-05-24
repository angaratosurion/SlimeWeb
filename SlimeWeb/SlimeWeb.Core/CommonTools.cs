using Microsoft.Extensions.Configuration;
using SlimeWeb.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Text;

namespace SlimeWeb.Core
{
    public class CommonTools
    {
        public static SlimeWebsUserManager usrmng = new SlimeWebsUserManager();
        public static FileRecordManager FileRecordManager = new FileRecordManager();
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
            //SlimeWeb.Core.Configuration.SlimeWeb.CoreSettingManager conf = new Configuration.SlimeWeb.CoreSettingManager();
            if (ex.GetBaseException() is ValidationException)
            {
                // ValidationErrorReporting((ValidationException)ex);


            }
            else
            {

                NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
               // logger.Fatal(ex);
                // if (conf.ExceptionShownOnBrowser() == true)
              //  {
                    //hrow (ex);
                    logger.TraceException(ex.Message, ex);
              //  }
            }

        }
        public static void ValidationErrorReporting(ValidationException ex)
        {
            //throw (ex);



            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            logger.Info(ex);


        }
        public static string GetSlimeWebVersion()
        {
            try
            { string ap;
                ap = Assembly.GetEntryAssembly().GetName().Version.ToString();

                return ap;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string GetSlimeWebCoreVersion()
        {
            try
            {
                string ap;
                ap = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                return ap;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void saveFileFRombase64string(string filedata, string filename)
        {
            try
            {
                if ( isEmpty(filedata) && isEmpty(filename))
                {
                    byte[] bytes = Convert.FromBase64String(filedata);
                    // Image image;
                    File.WriteAllBytes( filename, bytes);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }




    }
}
