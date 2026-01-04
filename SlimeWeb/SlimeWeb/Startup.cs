using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlimeWeb.Core;
using SlimeWeb.Core.App_Start;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using SlimeWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeWeb
{
    public class Startup:SlimeStartup
    {
        public Startup(IConfiguration configuration):base(configuration)
        {
            base.Configuration = configuration;
        }

        //public IConfiguration Configuration { get; }

        ///This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services = base.ConfigureServicesSlime(services);

           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


          //  app = base.ConfigureSlime(app, env);
            app = ConfigureRoutdsAndEndpoints(app,
                AppSettingsManager.GetEnableExtensionsSetting());
            bool errorshowing = AppSettingsManager.GetForceErrorShowingSetting();
            if (env.IsDevelopment() || errorshowing == true)
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
                {

                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                     
                });
            app = base.ConfigureSlime(app, env);




        }
    }
}
