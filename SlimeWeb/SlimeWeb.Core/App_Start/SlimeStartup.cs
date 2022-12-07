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
using Microsoft.AspNetCore.Http;
using SlimeWeb.Core.Managers;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using SlimeWeb.Core.Data.DBContexts;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using SlimeWeb.Core.CustomPolicy;
using SlimeWeb.Core.Tools;
using SlimeWeb.Core.Managers.Install;

namespace SlimeWeb.Core.App_Start
{
    public class SlimeStartup
    {
        string extensionsPath;
        //public static string WebRoot;
        bool Direcotrybrowse = false;
        public SlimeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
      
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceCollection ConfigureServicesSlime(IServiceCollection services)
        {
            if (AppSettingsManager.GetDBEngine() == enumDBEngine.MSQLServer.ToString())
            {
                services.AddDbContext<SlimeDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
            }
            else if (AppSettingsManager.GetDBEngine() == enumDBEngine.MySQl.ToString())
            {

                services.AddDbContext<SlimeDbContext>(options =>
                    options.UseMySQL(
                        Configuration.GetConnectionString("DefaultConnection")) ,ServiceLifetime.Transient);
            }
                services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
                  .AddEntityFrameworkStores<SlimeDbContext>()
                  .AddDefaultTokenProviders();
                services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));
                services.ConfigureApplicationCookie(options =>
                {
                    options.Cookie.Name = ".SlimeWeb";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                });
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(SlimeWebsUserManager.AdminRoles, policy =>
                {
                    policy.RequireRole(SlimeWebsUserManager.AdminRoles);
                    //  policy.RequireClaim("Administration", "Administration");

                });
            });
            services.AddTransient<IAuthorizationHandler, AllowUsersHandler>();
            services.AddAuthorization(opts =>
                {
                    opts.AddPolicy("AllowTom", policy =>
                    {
                        policy.AddRequirements(new AllowUserPolicy("tom"));
                    });
                });

            services.AddTransient<IAuthorizationHandler, AllowPrivateHandler>();
            services.AddAuthorization(opts =>
                {
                    opts.AddPolicy("PrivateAccess", policy =>
                    {
                        policy.AddRequirements(new AllowPrivatePolicy());
                    });
                });





            services.AddMvcCore().AddControllersAsServices()
                .AddRazorPages();
            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //  .AddEntityFrameworkStores<SlimeDbContext>();
            //this.extensionsPath = Path.Combine(hostingEnvironment.ContentRootPath, configuration["Extensions:Path"]);
           // Console.WriteLine("Code Base: {0}", System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            this.extensionsPath = Path.Combine(FileSystemManager.GetAppRootBinaryFolderAbsolutePath(), AppSettingsManager.GetExtetionPath());
            Console.WriteLine("Applications's Root  Path : {0}", FileSystemManager.GetAppRootBinaryFolderAbsolutePath());
            Console.WriteLine("Extention's Path : {0}",this.extensionsPath);
                //Configuration["Extensions:Path"];
            if (string.IsNullOrWhiteSpace(extensionsPath) == false)
            {
                if(Directory.Exists(this.extensionsPath)==false)
                {
                    Directory.CreateDirectory(this.extensionsPath);
                }
                services.AddExtCore(this.extensionsPath);
            }
            services.Configure<StorageContextOptions>(options =>
            {
               // options.MigrationsAssembly = typeof(DesignTimeStorageContextFactory).GetTypeInfo().Assembly.FullName;

            });
            //services.AddScoped<IStorageContext, SlimeDbContext>();
            services.AddTransient<IStorageContext, SlimeDbContext>();
            //services.AddSingleton<IStorageContext, SlimeDbContext>();
            //   DesignTimeStorageContextFactory.Initialize(services.BuildServiceProvider());

            services.AddSingleton<IEmailSender, EmailSender>();
            Direcotrybrowse = AppSettingsManager.GetAllowDirectoryBrowseSetting();
            if (Direcotrybrowse)
            {
                services.AddDirectoryBrowser();
            }

            services.Configure<KestrelServerOptions>(
               Configuration.GetSection("Kestrel"));
           
            return services;




        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void ConfigureSlime(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.extensionsPath = Path.Combine(FileSystemManager.GetAppRootBinaryFolderAbsolutePath(), AppSettingsManager.GetExtetionPath());
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
            // app.UseStaticFiles();
            if (Directory.Exists(FileSystemManager.GetAppRootDataFolderAbsolutePath())==false)
            {
                Directory.CreateDirectory(FileSystemManager.GetAppRootDataFolderAbsolutePath());
            }
            //if (Directory.Exists(Path.Combine(env.ContentRootPath, "wwwroot", FileSystemManager.AppDataDir)) == false)
            //{
            //    Directory.CreateDirectory(Path.Combine(env.ContentRootPath, "wwwroot", FileSystemManager.AppDataDir));
            //}
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath(), FileSystemManager.AppDataDir)),
                RequestPath = "/" + FileSystemManager.AppDataDir

            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath())),
                RequestPath = ""
            }); ;
            string pathbase;
            pathbase = AppSettingsManager.GetPathBase();
            app.UsePathBase(pathbase);


            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //  Path.Combine(env.ContentRootPath, "wwwroot")),
            //    RequestPath = ""
            //});







            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SlimeDbContext>();
                
               
                //return View();
                
                              
                                
                 //
                //
                bool createdb=false, migratedb = false, enablefileserver = false;
                createdb = AppSettingsManager.GetDataBaseCreationSetting();
                migratedb = AppSettingsManager.GetDataBaseMigrationSetting();
            

                CommonTools.usrmng = new SlimeWebsUserManager(serviceScope.ServiceProvider);

                //if (createdb && migratedb )
                //{

                //    context.Database.EnsureCreated();
                //    context.Database.Migrate();
                //}
                 if (createdb &&!migratedb)
                {
                    context.Database.EnsureCreated();
                }
                else if (migratedb)
                {
                    context.Database.Migrate();
                }
                enablefileserver = AppSettingsManager.GetEnableFileServer();
                if (enablefileserver)
                {
                    //app.UseFileServer(Direcotrybrowse);
                    app.UseFileServer(new FileServerOptions
                    {
                       
                        FileProvider = new PhysicalFileProvider(
            Path.Combine(FileSystemManager.GetAppRootBinaryFolderAbsolutePath(),"wwwroot", FileSystemManager.AppDataDir)),
                        RequestPath = "/"+ FileSystemManager.AppDataDir,
                        EnableDirectoryBrowsing = Direcotrybrowse
                    });

                }
                if (AppSettingsManager.GetisFirstRun())
                {
                    

                    InstallManager installManager = new InstallManager(serviceScope.ServiceProvider );
                    installManager.CrreateInitalAdmin();
                }
            }
            //if (!Directory.Exists(extensionsPath))
            //{
            //    Directory.CreateDirectory(extensionsPath);
            //}


            app.UseExtCore();

            NavigationManager.AddDefaultMenusOnTopMenu();
            NavigationManager.AddDefaultMenusOnBottomMenu();
           


        }
         


}
    }

