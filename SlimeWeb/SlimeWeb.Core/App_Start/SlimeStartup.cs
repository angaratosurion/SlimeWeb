using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data;
using ExtCore.WebApplication.Extensions;
using System.IO;
using ExtCore.Data.EntityFramework;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity.UI.Services;
using SlimeWeb.Core.Services;

namespace SlimeWeb.Core.App_Start
{
    public class SlimeStartup
    {
        string extensionsPath;
        public SlimeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
      
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceCollection ConfigureServicesSlime(IServiceCollection services)
        {
            services.AddDbContext<SlimeDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
              .AddEntityFrameworkStores<SlimeDbContext>()
              .AddDefaultTokenProviders();
            services.AddMvcCore().AddControllersAsServices()
                .AddRazorPages();
            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //  .AddEntityFrameworkStores<SlimeDbContext>();
            //this.extensionsPath = Path.Combine(hostingEnvironment.ContentRootPath, configuration["Extensions:Path"]);
            this.extensionsPath = Configuration["Extensions:Path"];
            if (string.IsNullOrWhiteSpace(extensionsPath) == false)
            {
                
                services.AddExtCore(this.extensionsPath);
            }
            services.Configure<StorageContextOptions>(options =>
            {
               // options.MigrationsAssembly = typeof(DesignTimeStorageContextFactory).GetTypeInfo().Assembly.FullName;

            });
            services.AddScoped<IStorageContext,SlimeDbContext>();
            //   DesignTimeStorageContextFactory.Initialize(services.BuildServiceProvider());
          
            services.AddSingleton<IEmailSender, EmailSender>();


            return services;




        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void ConfigureSlime(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.extensionsPath = Path.Combine(env.ContentRootPath, Configuration["Extensions:Path"]);
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    //app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //   // The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //} 

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();

            //app.UseRouting();

            //app.UseAuthentication();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //    endpoints.MapRazorPages();
            //});
           
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SlimeDbContext>();

                context.Database.EnsureCreated();
                context.Database.Migrate();
            }
            if (!Directory.Exists(extensionsPath))
            {
                Directory.CreateDirectory(extensionsPath);
            }
            app.UseExtCore();
        }
                

        }
    }

