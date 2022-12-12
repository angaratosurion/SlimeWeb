using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;

namespace SlimeWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CommonTools.CreateLogger();
            CreateHostBuilder(args)
                //.UseSerilog((context, services, configuration) => configuration
                //.ReadFrom.Configuration(context.Configuration)
                //.ReadFrom.Services(services)
                //.Enrich.FromLogContext()

            //.WriteTo.File(new CompactJsonFormatter(), "./wwwroot/AppData/logs/logs.json"))
            ////.CreateBootstrapLogger())
               .UseSerilog(CommonTools.logger)
                
                .Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                  
                        webBuilder

                        //.ConfigureKestrel(serverOptions =>
                        //{
                        //    // Set properties and call methods on options
                        //})
                        .UseStartup<Startup>()
                        //   webBuilder.UseStartup<Startup>()//;
                        //.UseContentRoot(AppContext.BaseDirectory)


                        ;
                    


                });
    }
}
