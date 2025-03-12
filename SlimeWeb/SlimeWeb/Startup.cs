using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlimeWeb.Core.App_Start;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data;
using SlimeWeb.Core;
using SlimeWeb.Core.Managers;
using Microsoft.AspNetCore.Mvc;

namespace SlimeWeb
{
    public class Startup:SlimeStartup
    {
        public Startup(IConfiguration configuration):base(configuration)
        {
          Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        ///This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           services= base.ConfigureServicesSlime(services);
            //services.AddDbContext<SlimeDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
           
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            
            app=base.ConfigureSlime(app, env);
            bool errorshowing = AppSettingsManager.GetForceErrorShowingSetting();
            if (env.IsDevelopment() || errorshowing==true)
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
            //app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
           
            //app =  ConfigureSlime(app, env);
            
            app.UseEndpoints(endpoints =>
                {

                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                    endpoints.MapControllers();
                });

            app = ConfigureRoutdsAndEndpoints(app, AppSettingsManager.GetEnableExtensionsSetting());


        }
    }
}
