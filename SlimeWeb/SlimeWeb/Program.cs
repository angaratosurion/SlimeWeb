using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using SlimeWeb.Core.Tools;

namespace SlimeWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {  CommonTools.CreateLogger();
            CreateHostBuilder(args)                
                
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
