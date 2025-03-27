using Microsoft.Extensions.Configuration;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SlimeWeb.Core.Managers
{
    public static class AppSettingsManager
    {
        static string pathwithextention;//= System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
       // static string path;//= System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\", "");
                           //return View();

        static ConfigurationBuilder builder;
        static IConfigurationRoot config;//= builder.Build();//
        public static void Init()
        {
            try
            {
                pathwithextention = System.Reflection.Assembly.GetExecutingAssembly()
                    .CodeBase;

                string path = "";//= System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\", "");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    path = System.IO.Path.GetDirectoryName(pathwithextention).
                        Replace("file:\\", "");
                }
                else
                {
                    path = System.IO.Path.GetDirectoryName(pathwithextention).
                        Replace("file:", "");
                }
                //return View();
                builder = (ConfigurationBuilder)new ConfigurationBuilder()
                          .SetBasePath(path)
                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //  var config = builder.Build();//


                config = builder.Build();//
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                 
            }

        }
        public static string GetDefaultAdminUserName()
        {
            try
            {
                Init();
                return config.GetValue<string>("ApppSettings:AdminUserName");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return null;
            }
        }
        public static string GetDefaultAdminUserPassword()
        {
            try
            {
                Init();
                return config.GetValue<string>("ApppSettings:AdminUserPassword");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return null;
            }
        }
        public static bool GetisFirstRun()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:firstRun");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return false;
            }
        }
       
        public static string GetPathBase()
        {
            try
            {
                Init();
                return config.GetValue<string>("ApppSettings:PathBase");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public static string GetDefaultController()
        {
            try
            {
                Init();
                return config.GetValue<string>("ApppSettings:DefaultRoot:Controller");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return null;
            }
        }
        public static bool GetHostedInSubFolderSetting()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:HostedInSubFolder");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return false;
            }
        }
        public static bool GetDataBaseMigrationSetting()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:DataBaseMigration");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return false;
            }
        }
        public static bool GetDataBaseCreationSetting()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:DataBaseCreation");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return false;
            }
        }
        public static bool GetForceErrorShowingSetting()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:ForceErrorShowing");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return false;
            }
        }
        public static string GetExtetionPath()
        {
            try
            {
                Init();

                return config["Extensions:Path"];
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return null;
            }
        }
        public static string GetDefaultConnectionString(string providername)
        {
            try
            {
                Init();
                //SqlServerConnection
                if (string.IsNullOrEmpty(providername))
                {
                    return null;
                }

                return config.GetValue<string>("ConnectionStrings:" + providername + "Connection");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return null;
            }
        }
        public static bool GetAllowDirectoryBrowseSetting()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:AllowDirectoryBrowse");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return false;
            }

        }
        public static bool GetEnableFileServer()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:EnableFileServer");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public static string GetAppWideCMSEngine()
        {
            try
            {
                Init();
                return config.GetValue<string>("ApppSettings:AppWideCMSEngine");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return null;
            }
        }
        public static string GetDBEngine()
        {
            try
            {
                Init();
                return config.GetValue<string>("ApppSettings:DBEngine");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex); 
                return null;
            }
        }
        public static bool GetEnableExtensionsSetting()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:EnableExtensions");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public static bool GetEnableExtensionsExtCoreSetting()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:EnableExtensionsExtCore");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public static bool GetEnableExtensionsSlimeWebSetting()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:EnableExtensionsSlimeWeb");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public static bool GetRuningOnDeveLoperMachineSetting()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:RuningOnDeveLoperMachine");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public static bool GetCompileOnRuntime()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:CompileOnRuntime");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public static bool GetEnablePagination()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:EnablePagination");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public static bool GetAllowChangingManagers()
        {
            try
            {
                Init();
                return config.GetValue<bool>("ApppSettings:AllowChangingManagers");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public static string GetDefaultPostManager(string name)
        {
            try
            {
                Init();
                //SqlServerConnection
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }

                return config.GetValue<string>("ApppSettings:DefaultManagers:PostManager");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public static string GetDefaultFileManager(string  name)
        {
            try
            {
                Init();
                //SqlServerConnection
                if (string.IsNullOrEmpty( name))
                {
                    return null;
                }

                return config.GetValue<string>("ApppSettings:DefaultManagers:FileManager");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public static string GetAccesManager(string   name)
        {
            try
            {
                Init();
                //SqlServerConnection
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }

                return config.GetValue<string>("ApppSettings:DefaultManagers:AccesManager");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public static string GetDefaultPagesManager(string  name)
        {
            try
            {
                Init();
                //SqlServerConnection
                if (string.IsNullOrEmpty( name))
                {
                    return null;
                }

                return config.GetValue<string>("ApppSettings:DefaultManagers:PagesManager");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public static string GetDefaultCategoryManager(string   name)
        {
            try
            {
                Init();
                //SqlServerConnection
                if (string.IsNullOrEmpty( name))
                {
                    return null;
                }

                return config.GetValue<string>("ApppSettings:DefaultManagers:CategoryManager");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }



    }
}
