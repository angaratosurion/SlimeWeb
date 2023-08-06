using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SlimeWeb.Core.SDK.Interfaces;
using SlimeWeb.Core.Tools;
using System.Collections.Generic;
using System.Reflection;

namespace SlimeWeb.Core.SDK
{
    public   class SlimePluginManager
    {
        public static List<ISlimeServiceExtension> LoadServicesPlugins(string relativePath)
        {
            try
            {
                string pluginLocation = relativePath;
                List<ISlimeServiceExtension> ap = null;
                if (CommonTools.isEmpty(pluginLocation) != true)
                {
                    ap = new List<ISlimeServiceExtension>();
                    var files = Directory.GetFiles(pluginLocation);
                    foreach (var file in files)
                    {
                        var plg = LoadPlugin(file);
                        if (plg != null)
                        {
                            var serv = CreateSericeExtesion(plg);
                            if (serv != null)
                            {
                                ap.Add (serv);
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
       static  ISlimeServiceExtension CreateSericeExtesion(Assembly assembly)
        {
            try
            {
                int count = 0;
                ISlimeServiceExtension  ap = null;

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(ISlimeServiceExtension).IsAssignableFrom(type))
                    {
                        ISlimeServiceExtension result = Activator.CreateInstance(type) as ISlimeServiceExtension;
                        if (result != null)
                        {
                            count++;

                            return ( ISlimeServiceExtension )result;

                            
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
                    if (typeof(ISlimeServiceExtension).IsAssignableFrom(type))
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
    }
}