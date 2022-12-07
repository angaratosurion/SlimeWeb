using Microsoft.Extensions.Configuration;
using SlimeWeb.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SlimeWeb.Core.Tools
{
    public class CommonTools
    {
        public static SlimeWebsUserManager usrmng;
      
        //public static FileRecordManager FileRecordManager = new FileRecordManager(usrmng.Context);
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
                Console.WriteLine(ex.ToString());
                //throw (ex);
                 //   logger.TraceException(ex.Message, ex);
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
        public static string GetSlimeWebDeveloper()
        {
            try
            {
                string ap;
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                AssemblyCompanyAttribute attribute = null;
                if (attributes.Length > 0)
                {
                    attribute = attributes[0] as AssemblyCompanyAttribute;
                }
                ap = attribute.Company;

                return ap;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string GetSlimeWebCopyright()
        {
            try
            {
                string ap;
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                AssemblyCopyrightAttribute attribute = null;
                if (attributes.Length > 0)
                {
                    attribute = attributes[0] as AssemblyCopyrightAttribute;
                }
                ap = attribute.Copyright;

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
        public static string GetSlimeWebCoreDeveloper()
        {
            try
            {
                string ap;
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                AssemblyCompanyAttribute attribute = null;
                if (attributes.Length > 0)
                {
                    attribute = attributes[0] as AssemblyCompanyAttribute;
                }
                ap = attribute.Company;

                return ap;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string GetSlimeWebCoreCopyright()
        {
            try
            {
                string ap;
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                AssemblyCopyrightAttribute attribute = null;
                if (attributes.Length > 0)
                {
                    attribute = attributes[0] as AssemblyCopyrightAttribute;
                }
                ap = attribute.Copyright;

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
