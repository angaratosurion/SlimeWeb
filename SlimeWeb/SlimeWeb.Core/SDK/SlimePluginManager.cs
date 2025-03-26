using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.SDK.Interfaces;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using IExtension = SlimeWeb.Core.SDK.Interfaces.IExtension;

namespace SlimeWeb.Core.SDK
{
    public   class SlimePluginManager
    {
        public static List<ExtCore.Infrastructure.IExtension>
            GetSlimeWebExtCoreExtensionInfo()
        {
            try
            {
                List<ExtCore.Infrastructure.IExtension> ap = null; ;
                if (AppSettingsManager.GetEnableExtensionsExtCoreSetting() &&
                           AppSettingsManager.GetEnableExtensionsSlimeWebSetting() == false)
                {

                    ap = ExtensionManager.GetInstances<ExtCore.Infrastructure.IExtension>().ToList();
                }


                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
        public static List<IExtension> GetSlimeWebSDKExtensionInfo( )
        {
            try
            {
                List<IExtension> ap = null; ;
                string pluginLocation = Path.Combine(FileSystemManager.
                    GetAppRootBinaryFolderAbsolutePath(),
                    AppSettingsManager.GetExtetionPath());
                if (!AppSettingsManager.GetEnableExtensionsExtCoreSetting() &&
                           AppSettingsManager.GetEnableExtensionsSlimeWebSetting() != false)
                {

                    EnumerationOptions enumerationOptions = new EnumerationOptions();
                    enumerationOptions.RecurseSubdirectories = true;
                    var files = Directory.GetFiles(pluginLocation, "*.dll",
                        enumerationOptions);
                    ap = new List<IExtension>();
                    foreach (var file in files)
                    {
                        var plg = LoadPlugin(file);
                        if (plg != null)
                        {

                            var serv = CreateExtensionInfo(plg);
                            if (serv != null)
                            {

                                ap.Add(serv);
                                 
                            }
                        }

                    }

                
            }


                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
        public static List<IConfigureServicesAction> LoadServicesPlugins(string relativePath, 
            IServiceCollection services,
            IServiceProvider serviceProvider)
        {
            try
            {
                string pluginLocation = relativePath;
                List<IConfigureServicesAction> ap = null;
                if (CommonTools.isEmpty(pluginLocation) != true )
                {
                   
                    ap = new List<IConfigureServicesAction>();
                    EnumerationOptions enumerationOptions  = new EnumerationOptions();
                    enumerationOptions.RecurseSubdirectories = true;
                    var files = Directory.GetFiles(pluginLocation,"*.dll",enumerationOptions);
                    foreach (var file in files)
                    {
                        var plg = LoadPlugin(file);
                        if (plg != null)
                        {
                            var serv = CreateSericeExtesion(plg);
                            if (serv != null)
                            {
                                ap.Add (serv);
                                serv.Execute(services, serviceProvider);
                            }
                        }

                    }

                }



                return ap;
            }


            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }

        }
        public static List<IUseEndpointsAction> LoadEndpointPlugins(string relativePath,
            IEndpointRouteBuilder endpointRouteBuilder, 
            IServiceProvider serviceProvider)
        {
            try
            {
                string pluginLocation = relativePath;
                List<IUseEndpointsAction> ap = null;
                if (CommonTools.isEmpty(pluginLocation) != true)
                {

                    ap = new List<IUseEndpointsAction>();
                    EnumerationOptions enumerationOptions = new EnumerationOptions();
                    enumerationOptions.RecurseSubdirectories = true;
                    var files = Directory.GetFiles(pluginLocation, "*.dll", enumerationOptions);
                    foreach (var file in files)
                    {
                        var plg = LoadPlugin(file);
                        if (plg != null)
                        {
                           
                            var serv = CreateEndpointsExtension(plg);
                            if (serv != null)
                            {
                               
                                ap.Add(serv);
                                serv.Execute(endpointRouteBuilder, serviceProvider);
                            }
                        }

                    }

                }



                return ap;
            }


            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }

        }
        public static List<IAddMvcAction> LoadAddMvcActionPlugins(string relativePath, 
            IMvcCoreBuilder mvcBuilder, 
            IServiceProvider serviceProvider)
        {
            try
            {
                string pluginLocation = relativePath;
                List<IAddMvcAction>  ap = null;
                if (CommonTools.isEmpty(pluginLocation) != true)
                {

                    ap = new List<IAddMvcAction> ();
                    EnumerationOptions enumerationOptions = new EnumerationOptions();
                    enumerationOptions.RecurseSubdirectories = true;
                    var files = Directory.GetFiles(pluginLocation, "*.dll", enumerationOptions);
                    foreach (var file in files)
                    {
                        var plg = LoadPlugin(file);
                        if (plg != null)
                        {
                            mvcBuilder.AddApplicationPart(plg);
                            var serv = CreateAddMvcActionExtesion(plg);
                            if (serv != null)
                            {

                                ap.Add(serv);
                                serv.Execute(mvcBuilder, serviceProvider);
                            }
                        }

                    }

                }



                return ap;
            }


            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }

        }
        public static List<IAddMvcAction> LoadAddMvcActionPlugins(string relativePath, IMvcBuilder mvcBuilder,
            IServiceProvider serviceProvider)
        {
            try
            {
                string pluginLocation = relativePath;
                List<IAddMvcAction> ap = null;
                if (CommonTools.isEmpty(pluginLocation) != true)
                {

                    ap = new List<IAddMvcAction>();
                    EnumerationOptions enumerationOptions = new EnumerationOptions();
                    enumerationOptions.RecurseSubdirectories = true;
                    var files = Directory.GetFiles(pluginLocation, "*.dll", enumerationOptions);
                    foreach (var file in files)
                    {
                        var plg = LoadPlugin(file);
                        if (plg != null)
                        {
                            mvcBuilder.AddApplicationPart(plg);
                            var serv = CreateAddMvcActionExtesion(plg);
                            if (serv != null)
                            {

                                ap.Add(serv);
                                serv.Execute((IMvcCoreBuilder)mvcBuilder, serviceProvider);
                            }
                        }

                    }

                }



                return ap;
            }


            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }

        }

        public static Assembly LoadPlugin(string relativePath)
        {
            try
            {
                string pluginLocation = relativePath;
                // Console.WriteLine($"Loading commands from: {pluginLocation}");
                PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
                return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }

        }
       static IConfigureServicesAction CreateSericeExtesion(Assembly assembly)
        {
            try
            {
                int count = 0;
                IConfigureServicesAction ap = null;

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IConfigureServicesAction).IsAssignableFrom(type))
                    {
                        IConfigureServicesAction result = Activator.CreateInstance(type) as IConfigureServicesAction;
                        if (result != null)
                        {
                            count++;

                            return (IConfigureServicesAction)result;

                            
                        }
                    }
                }
                return ap; ;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
            }
        static IAddMvcAction CreateAddMvcActionExtesion(Assembly assembly)
        {
            try
            {
                int count = 0;
                IAddMvcAction ap = null;

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IAddMvcAction).IsAssignableFrom(type))
                    {
                        IAddMvcAction  result = Activator.CreateInstance(type) as IAddMvcAction ;
                        if (result != null)
                        {
                            count++;

                            return (IAddMvcAction )result;


                        }
                    }
                }
                return ap; ;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        static IUseEndpointsAction? CreateEndpointsExtension(Assembly assembly)
        {
            try
            {
                int count = 0;
               IUseEndpointsAction ap = null;

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IUseEndpointsAction).IsAssignableFrom(type))
                    {
                        IUseEndpointsAction result = Activator.CreateInstance(type) as IUseEndpointsAction ;
                        if (result != null)
                        {
                            count++;
                            return (IUseEndpointsAction)result;
                        }
                    }
                }
                return ap; ;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        static IExtension? CreateExtensionInfo(Assembly assembly)
        {
            try
            {
                int count = 0;
                IExtension ap = null;

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IExtension).IsAssignableFrom(type))
                    {
                        IExtension result = Activator.CreateInstance(type) as IExtension;
                        if (result != null)
                        {
                            count++;
                            return (IExtension)result;
                        }
                    }
                }
                return ap; ;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

        public static List<IConfigureAction> LoadConfigurePlugins(string relativePath, IApplicationBuilder applicationBuilder, 
            IServiceProvider serviceProvider)
        {
            try
            {
                string pluginLocation = relativePath;
                List<IConfigureAction> ap = null;
                if (CommonTools.isEmpty(pluginLocation) != true)
                {

                    ap = new List<IConfigureAction>();
                    EnumerationOptions enumerationOptions = new EnumerationOptions();
                    enumerationOptions.RecurseSubdirectories = true;
                    var files = Directory.GetFiles(pluginLocation, "*.dll", enumerationOptions);
                    foreach (var file in files)
                    {
                        var plg = LoadPlugin(file);
                        if (plg != null)
                        {
                            var serv = CreateConfigureExtesion(plg);
                            if (serv != null)
                            {
                                ap.Add(serv);
                                serv.Execute(applicationBuilder, serviceProvider);
                            }
                        }

                    }

                }



                return ap;
            }


            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }

        }
        static IConfigureAction CreateConfigureExtesion(Assembly assembly)
        {
            try
            {
                int count = 0;
                IConfigureAction ap = null;

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IConfigureAction).IsAssignableFrom(type))
                    {
                        IConfigureAction result = Activator.CreateInstance(type) as IConfigureAction;
                        if (result != null)
                        {
                            count++;

                            return (IConfigureAction)result;


                        }
                    }
                }
                return (IConfigureAction)ap; ;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
    }
}