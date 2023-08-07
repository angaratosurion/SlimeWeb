using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.SDK.Interfaces;
using SlimeWeb.Core.Tools;
using System.Collections.Generic;
using System.Reflection;

namespace SlimeWeb.Core.SDK
{
    public   class SlimePluginManager
    {
        public static List<IConfigureServicesAction> LoadServicesPlugins(string relativePath, IServiceCollection services,
            IServiceProvider serviceProvider)
        {
            try
            {
                string pluginLocation = relativePath;
                List<IConfigureServicesAction> ap = null;
                if (CommonTools.isEmpty(pluginLocation) != true )
                {
                   
                    ap = new List<IConfigureServicesAction>();
                    var files = Directory.GetFiles(pluginLocation,"*.dll");
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
        static IEnumerable<Controller>? CreateControllerExtesion(Assembly assembly)
        {
            try
            {
                int count = 0;
                IEnumerable<Controller> ap = null;

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(Controller).IsAssignableFrom(type))
                    {
                        Controller  result = Activator.CreateInstance(type) as Controller ;
                        if (result != null)
                        {
                            count++;
                            return (IEnumerable< Controller >?)result;
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
                    var files = Directory.GetFiles(pluginLocation, "*.dll");
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