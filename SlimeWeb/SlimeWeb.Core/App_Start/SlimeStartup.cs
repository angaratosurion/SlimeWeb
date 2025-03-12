using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using ExtCore.Infrastructure;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SlimeWeb.Core.CustomPolicy;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Managers.Install;
using SlimeWeb.Core.Managers.Markups;
using SlimeWeb.Core.SDK;
using SlimeWeb.Core.SDK.Interfaces;
using SlimeWeb.Core.Services;
using SlimeWeb.Core.Tools;
using System;
using System.IO;

namespace SlimeWeb.Core.App_Start
{
    /// <summary>
    /// The startup class of SlimeWeb.Core library
    /// </summary>
    public class SlimeStartup
    {
        static string extensionsPath;
        //public static string WebRoot;
        bool Direcotrybrowse = false;
        static List<IConfigureServicesAction> slimeServicesExtension;
        static List<IConfigureAction> slimeExtension;
       public static IServiceCollection Services;
        static List<IAddMvcAction> addMvcActions;
        /// <summary>
        /// Iinitailizes the Startup claass of SlimeWeb  CMS
        /// </summary>
        /// <param name="configuration"></param>
        public SlimeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
      
        /// <summary>
        /// the property that includes the configuration of the cms
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// This method gets called by the runtime. 
        /// Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        // 
        public IServiceCollection ConfigureServicesSlime(IServiceCollection services)
        {
            try
            {
                if (AppSettingsManager.GetDBEngine() == enumDBEngine.SQLServer.ToString())
                {

                    services.AddDbContext<SlimeDbContext>(options =>
                        options.UseSqlServer(
                            Configuration.GetConnectionString("SqlServerConnection"), 
                            b => b.MigrationsAssembly("SlimeWeb.Core.Migrations.SQLServerMigrations")
                            ), ServiceLifetime.Transient);
                }
                else if (AppSettingsManager.GetDBEngine() == enumDBEngine.MySQl.ToString())
                {

                    services.AddDbContext<SlimeDbContext>(options =>
                        options.UseMySQL(
                            Configuration.GetConnectionString("MySQlConnection"), 
                            b => b.MigrationsAssembly("SlimeWeb.Core.Migrations.MySQLMigrations")),
                            ServiceLifetime.Singleton);
                }
                services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.
                RequireConfirmedAccount = true)
                  .AddEntityFrameworkStores<SlimeDbContext>()
                  .AddDefaultTokenProviders()//;
                 .AddDefaultUI();
                services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = 
                TimeSpan.FromHours(10));
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




               var  mvcopts = new MvcOptions();
                mvcopts.EnableEndpointRouting = false;
                //IMvcCoreBuilder mvcBuilder =
                //     services.AddMvcCore().AddControllersAsServices().AddRazorPages();
                IMvcBuilder mvcBuilder = services.AddMvc()
                    .AddControllersAsServices();
                 
                    ;
                //.AddMvcOptions(mvcopts);
              

                if (AppSettingsManager.GetCompileOnRuntime())
                {
                    services.AddControllersWithViews()
                        .AddRazorRuntimeCompilation();
                }
                else
                {
                    services.AddControllersWithViews();
                }
                
                services.AddRazorPages();
                services.AddControllers();
                 

                //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //  .AddEntityFrameworkStores<SlimeDbContext>();
                //extensionsPath = Path.Combine(hostingEnvironment.ContentRootPath, configuration["Extensions:Path"]);
                // Console.WriteLine("Code Base: {0}", System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                extensionsPath = Path.Combine(FileSystemManager.GetAppRootBinaryFolderAbsolutePath(),
                    AppSettingsManager.GetExtetionPath());
                Console.WriteLine("Applications's Root  Path : {0}",
                    FileSystemManager.GetAppRootBinaryFolderAbsolutePath());
                Console.WriteLine("Extention's Path : {0}", extensionsPath);
                //Configuration["Extensions:Path"];
                MarkUpManager.Init();
                if (AppSettingsManager.GetEnableExtensionsSetting())
                {
                    if (string.IsNullOrWhiteSpace(extensionsPath) == false)
                    {
                        if (Directory.Exists(extensionsPath) == false)
                        {
                            Directory.CreateDirectory(extensionsPath);
                        }
                        if (AppSettingsManager.GetEnableExtensionsExtCoreSetting() &&
                            AppSettingsManager.GetEnableExtensionsSlimeWebSetting() == false)
                        {
                            services.AddExtCore(extensionsPath, true);
                        }
                        else if (AppSettingsManager.GetEnableExtensionsExtCoreSetting() == false &&
                            AppSettingsManager.GetEnableExtensionsSlimeWebSetting() != false)
                        {
                            slimeServicesExtension = SlimePluginManager.
                                LoadServicesPlugins(extensionsPath,services,
                                services.BuildServiceProvider());
                            Services = services;
                           addMvcActions= SlimePluginManager.LoadAddMvcActionPlugins(extensionsPath, 
                               mvcBuilder, services.BuildServiceProvider());



                        }
                    }
                    services.Configure<StorageContextOptions>(options =>
                    {
                        // options.MigrationsAssembly = typeof(DesignTimeStorageContextFactory).GetTypeInfo().Assembly.FullName;

                    });
                     
                    services.AddTransient<IStorageContext, SlimeDbContext>();
                     

                    services.AddSingleton<IEmailSender, EmailSender>();
                    Direcotrybrowse = AppSettingsManager.GetAllowDirectoryBrowseSetting();
                    if (Direcotrybrowse)
                    {
                        services.AddDirectoryBrowser();
                    }

                    services.Configure<KestrelServerOptions>(
                       Configuration.GetSection("Kestrel"));

                   
                }
                return services;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return services;
            }




        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to 
        /// configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        // 
        public IApplicationBuilder ConfigureSlime(IApplicationBuilder app, IWebHostEnvironment env)
        {

            try
            {
                IApplicationBuilder tap = null;
                extensionsPath = Path.Combine(FileSystemManager.GetAppRootBinaryFolderAbsolutePath(), 
                    AppSettingsManager.GetExtetionPath());

                app.UseCookiePolicy();
              


                

                 
                app.UseAuthentication();
                app.UseAuthorization();
              

                // app.UseStaticFiles();
                if (Directory.Exists(FileSystemManager.GetAppRootDataFolderAbsolutePath()) == false)
                {
                    Directory.CreateDirectory(FileSystemManager.GetAppRootDataFolderAbsolutePath());
                }
                

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                    Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath(),
                    FileSystemManager.AppDataDir)),
                    RequestPath = "/" + FileSystemManager.AppDataDir

                });
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath())),
                    RequestPath = ""
                }); ;
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
               Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath(), "lib")),
                    RequestPath = "/lib"
                }); ;
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
               Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath(), "js")),
                    RequestPath = "/js"
                }); ;
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
               Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath() )),
                    RequestPath = "/wwwroot"
                }); ;
                string pathbase;
                pathbase = AppSettingsManager.GetPathBase();
                app.UsePathBase(pathbase);
                app=ConfigureRoutdsAndEndpoints(app, AppSettingsManager.GetEnableExtensionsSetting());







                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().
                    CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<SlimeDbContext>();
                    
                    bool createdb = false, migratedb = false, enablefileserver = false;
                    createdb = AppSettingsManager.GetDataBaseCreationSetting();
                    migratedb = AppSettingsManager.GetDataBaseMigrationSetting();

                    Console.WriteLine(context.Database.GetConnectionString());
                    CommonTools.usrmng = new SlimeWebsUserManager(serviceScope.ServiceProvider);

                   
                    if (createdb && !migratedb)
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
                Path.Combine(FileSystemManager.GetAppRootBinaryFolderAbsolutePath(), "wwwroot", 
                FileSystemManager.AppDataDir)),
                            RequestPath = "/" + FileSystemManager.AppDataDir,
                            EnableDirectoryBrowsing = Direcotrybrowse
                        });

                    }
                    if (AppSettingsManager.GetisFirstRun())
                    {


                        InstallManager installManager = new InstallManager(serviceScope.ServiceProvider);
                        installManager.CrreateInitalAdmin();
                    }
                }


                if (AppSettingsManager.GetEnableExtensionsSetting())
                {
                    if (AppSettingsManager.GetEnableExtensionsExtCoreSetting() &&
                           AppSettingsManager.GetEnableExtensionsSlimeWebSetting() == false)
                    {
                        app.UseExtCore();
                    }
                    else if (AppSettingsManager.GetEnableExtensionsExtCoreSetting() == false &&
                           AppSettingsManager.GetEnableExtensionsSlimeWebSetting() != false)
                    {

                        slimeExtension = SlimePluginManager.LoadConfigurePlugins(extensionsPath, app,
                            app.ApplicationServices);
                        app.UseRouting();

                        app.UseForwardedHeaders(new ForwardedHeadersOptions
                        {
                            ForwardedHeaders = ForwardedHeaders.XForwardedFor
                            | ForwardedHeaders.XForwardedProto
                        });
                    }
                }

                       
                         
                       



                       tap = app;
                         
                   

                    NavigationManager.AddDefaultMenusOnTopMenu();
                    NavigationManager.AddDefaultMenusOnBottomMenu();
                    

                
                
                 
                tap = app;
                return tap;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return app; ;
            }
            }

        //public    void  Configure(IApplicationBuilder app, IWebHostEnvironment env)

        //{
        //    ConfigureSlime(app, env);
             
        //}
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services = ConfigureServicesSlime(services);
        //}
        public static IApplicationBuilder ConfigureRoutdsAndEndpoints(IApplicationBuilder app, bool enableExtensions )
        {
            try
            {
                IApplicationBuilder tapp;
             //   if ( app !=null)
                {
                    string pathbase;
                    app.UseRouting();
                    app.UseAuthorization();
                    pathbase = AppSettingsManager.GetPathBase();
                    if (enableExtensions)
                    {
                        app.UseEndpoints(endpoints =>
                        {

                            var Endpointplugins = SlimePluginManager.LoadEndpointPlugins(extensionsPath,
                                endpoints, app.ApplicationServices);
                            endpoints.MapControllerRoute(
                             name: "default",
                            pattern:"{controller=Home}/{action=Index}/{id?}");
                            endpoints.MapControllerRoute(
                             name: "Post",
                            pattern:   "{controller=Posts}/{action=Index}/{name}/{page?}");

                            endpoints.MapRazorPages();
                            endpoints.MapControllers();

                        });
                        app.UseMvc(routes =>
                        {
                            routes.MapRoute(name: "default",
                            template:  "{ controller = Home}/{ action = Index}/{ id?}");
                            routes.MapRoute(name: "Post",
                            template:  "{ controller = Posts}/{action=Index}/{name}/{page?}");
                        });
                    }
                    else
                    {
                        //app.UseEndpoints(endpoints =>
                        //{

                        //    //var Endpointplugins = SlimePluginManager.LoadEndpointPlugins(extensionsPath,
                        //    //    endpoints, app.ApplicationServices);
                        //    endpoints.MapControllerRoute(
                        //     name: "default",
                        //    pattern: pathbase + "/{controller=Home}/{action=Index}/{id?}");
                        //    endpoints.MapControllerRoute(
                        //     name: "Post",
                        //    pattern: pathbase + "/{controller=Posts}/{action=Index}/{name}/{page?}");

                        //    endpoints.MapRazorPages();
                        //    endpoints.MapControllers();

                        //});
                        app.UseMvc(routes =>
                        {
                            routes.MapRoute(name: "default",
                            template: pathbase + "/" + "{ controller = Home}/{ action = Index}/{ id?}");
                            routes.MapRoute(name: "Post",
                            template: pathbase + "/" + "{ controller = Posts}/{action=Index}/{name}/{page?}");
                        });
                    }
                    tapp = app;
                    return app;
                }
            }
            catch (Exception ex )
            {
                CommonTools.ErrorReporting(ex);
                return app;

            }
        }

    }
    
    }

