using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Fluent;
using NLog.Web;
using SlimeWeb.Core.Managers;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;

namespace SlimeWeb.Core.Tools
{
    public class CommonTools
    {
        public static SlimeWebsUserManager usrmng;
        public static Logger logger;//= LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        public static void CreateLogger()
        {
            try
            {
                var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
                var tlogger = LogManager.Setup()
                       .LoadConfigurationFromSection(config)
                       .GetCurrentClassLogger();
                
                logger = tlogger;
            }
            catch (Exception ex)
            {
                 ErrorReporting(ex);

                 
            }

        }
       
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
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

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
                logger.Fatal(ex);

            }
            else
            {


                CreateLogger();


                //(new CompactJsonFormatter());



                //.ReadFrom.Services(services)
                //  .Enrich.FromLogContext()

                // .WriteTo.File(new CompactJsonFormatter(), "/wwwroot/AppData/logs/logs.json"))
                //.CreateBootstrapLogger())

                logger.Fatal(ex.ToString());
                NLog.LogManager.Flush();
                // logger.Fatal(ex);
                // if (conf.ExceptionShownOnBrowser() == true)
                //  {
              //  Console.WriteLine(ex.ToString());
                //throw (ex);
                 //   logger.TraceException(ex.Message, ex);
              //  }
            }

        }
        public static void ValidationErrorReporting(ValidationException ex)
        {
            //throw (ex);
            logger.Error(ex, "Validation Exception");


          


        }
        public static string GetSlimeWebVersion()
        {
            try
            { string ap;
                ap = Assembly.GetEntryAssembly().GetName().Version.ToString();

                return ap;

            }
            catch (Exception ex)
            {
               ErrorReporting(ex);

                return null;
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
            catch (Exception ex)
            {
               ErrorReporting(ex);

                return null;
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
            catch (Exception ex)
            {
                ErrorReporting(ex);

                return null;
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
            catch (Exception ex)
            {
                ErrorReporting(ex);

                return null;
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
            catch (Exception ex)
            {
                ErrorReporting(ex);

                return null;
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
            catch (Exception ex)
            {
                ErrorReporting(ex);

                return null;
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
            catch (Exception ex)
            {
                ErrorReporting(ex);

                
            }
        }




    }
}
