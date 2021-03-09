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
            services.AddDbContext<SlimeDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();


            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //  .AddEntityFrameworkStores< ApplicationDbContext > ();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            base.ConfigureSlime(app, env);
           if (env.IsDevelopment())
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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            string pathwithextention = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string path = System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\", "");
            //return View();
            var builder = new ConfigurationBuilder()
                            .SetBasePath(path)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var config = builder.Build();//
                                         //
            string webapppname= config.GetValue<string>("ApppSettings:WebAppName");
            if (CommonTools.isEmpty(webapppname)==false)
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern:webapppname+"/"+ "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                });
            }
            else
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                });
            }
        }
    }
}
