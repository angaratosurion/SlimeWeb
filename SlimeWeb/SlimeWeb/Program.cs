using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SlimeWeb.Core.Tools;
using NLog;
using NLog.Web;

namespace SlimeWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {  CommonTools.CreateLogger();
            CreateHostBuilder(args)
                .UseNLog()
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
