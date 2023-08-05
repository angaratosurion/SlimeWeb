using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SlimeWeb.Core.Tools;
using NLog;
using NLog.Web;
using SlimeWeb.Core.App_Start;

namespace SlimeWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {  CommonTools.CreateLogger();
            CreateHostBuilder(args)
                .UseNLog()
                .Build() //;
              .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                  
                        webBuilder


                        .UseStartup<Startup>()

                         

                        ;
                    


                });
    }
}
