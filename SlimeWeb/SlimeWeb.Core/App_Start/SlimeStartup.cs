using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using ExtCore.WebApplication.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SlimeWeb.Core.CustomPolicy;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.MySQL;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Managers.Install;
using SlimeWeb.Core.Managers.Managment;
using SlimeWeb.Core.Managers.Markups;
using SlimeWeb.Core.SDK;
using SlimeWeb.Core.SDK.Interfaces;
using SlimeWeb.Core.Services;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Services.Description;

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
        public static IApplicationBuilder AppBuilder;
        static List<IAddMvcAction> addMvcActions;
        public static SlimeWebsUserManager  SlimeWebsUserManager;
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
        public IConfiguration Configuration { get; set; }
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
                services.AddControllersWithViews();
                     services.AddRazorPages();
                    services.AddControllers();

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
                        options.UseMySql(Configuration.GetConnectionString("MySQlConnection"),
                         ServerVersion.AutoDetect(Configuration.GetConnectionString("MySQlConnection")),
                         b => b.MigrationsAssembly("SlimeWeb.Core.Migrations.MySQLMigrations").
                         MigrationsHistoryTable("EF_History")
                                )
                        .ReplaceService<IHistoryRepository, CustomMySqlHistoryRepository>()); 
                    // Use your custom repository);





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




                var mvcopts = new MvcOptions();
                mvcopts.EnableEndpointRouting = false;
                //IMvcCoreBuilder mvcBuilder =
                //     services.AddMvcCore().AddControllersAsServices().AddRazorPages();
                IMvcBuilder mvcBuilder = services.AddMvc();
                    //.AddControllersAsServices();

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

                //services.AddRazorPages();
                //services.AddControllers();


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
                                LoadServicesPlugins(extensionsPath, services,
                                services.BuildServiceProvider());
                            Services = services;
                            addMvcActions = SlimePluginManager.LoadAddMvcActionPlugins(extensionsPath,
                                mvcBuilder, services.BuildServiceProvider());
                            SlimePluginManager.LoadExternalControllers(
                                extensionsPath,
                                mvcBuilder,services);



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

                services.AddScoped<SlimeWebsUserManager>();
                if (AppSettingsManager.GetAllowChangingManagers())
                {
                    GroupedManagers groupedManagers =
                        ManagerManagment.GetDefaultManagger();
                    if (groupedManagers != null)
                    {

                        if (groupedManagers.PostManager == null)
                        {
                             


                        }
                    }
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
                AppBuilder = app;

                extensionsPath = Path.Combine(
                    FileSystemManager.GetAppRootBinaryFolderAbsolutePath(),
                    AppSettingsManager.GetExtetionPath()
                );

                app.UseCookiePolicy();

                app.UseAuthentication();
                app.UseAuthorization();

                // Ensure data folder exists
                if (!Directory.Exists(FileSystemManager.GetAppRootDataFolderAbsolutePath()))
                {
                    Directory.CreateDirectory(FileSystemManager.GetAppRootDataFolderAbsolutePath());
                }

                // Static files
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath(), FileSystemManager.AppDataDir)
                    ),
                    RequestPath = "/" + FileSystemManager.AppDataDir
                });

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath())
                    ),
                    RequestPath = ""
                });

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath(), "lib")
                    ),
                    RequestPath = "/lib"
                });

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath(), "js")
                    ),
                    RequestPath = "/js"
                });

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(FileSystemManager.GetAppRootFolderAbsolutePath())
                    ),
                    RequestPath = "/wwwroot"
                });

                // PathBase
                string pathbase = AppSettingsManager.GetPathBase();
                app.UsePathBase(pathbase);

                // ❗ IMPORTANT FIX:
                // DO NOT call UseRouting() or MapControllers() here.
                // They must be called ONLY ONCE in Program.cs.
                app = ConfigureRoutdsAndEndpoints(app, AppSettingsManager.GetEnableExtensionsSetting());

                // Database setup
                using (var serviceScope = app.ApplicationServices
                    .GetService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<SlimeDbContext>();

                    bool createdb = AppSettingsManager.GetDataBaseCreationSetting();
                    bool migratedb = AppSettingsManager.GetDataBaseMigrationSetting();

                    if (createdb && !migratedb)
                        context.Database.EnsureCreated();
                    else if (migratedb)
                        context.Database.Migrate();

                    bool enablefileserver = AppSettingsManager.GetEnableFileServer();
                    if (enablefileserver)
                    {
                        app.UseFileServer(new FileServerOptions
                        {
                            FileProvider = new PhysicalFileProvider(
                                Path.Combine(
                                    FileSystemManager.GetAppRootBinaryFolderAbsolutePath(),
                                    "wwwroot",
                                    FileSystemManager.AppDataDir
                                )
                            ),
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

                // Extensions
                if (AppSettingsManager.GetEnableExtensionsSetting())
                {
                    if (AppSettingsManager.GetEnableExtensionsExtCoreSetting() &&
                        !AppSettingsManager.GetEnableExtensionsSlimeWebSetting())
                    {
                        app.UseExtCore();
                    }
                    else if (!AppSettingsManager.GetEnableExtensionsExtCoreSetting() &&
                              AppSettingsManager.GetEnableExtensionsSlimeWebSetting())
                    {
                        
                        // ❗ FIX: DO NOT call UseRouting() here.
                        app.UseForwardedHeaders(new ForwardedHeadersOptions
                        {
                            ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                               ForwardedHeaders.XForwardedProto
                        });
                    }
                }

                NavigationManager.AddDefaultMenusOnTopMenu();
                NavigationManager.AddDefaultMenusOnBottomMenu();

                var scope = app.ApplicationServices.CreateScope();
                
                    var usermanager = scope.ServiceProvider
                    .GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();
                var singmanager = scope.ServiceProvider
                    .GetRequiredService<Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser>>();
                var rolmnger = scope.ServiceProvider
                    .GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole>>();
                var db = scope.ServiceProvider
                    .GetRequiredService<SlimeDbContext>();

                CommonTools.usrmng = new SlimeWebsUserManager(
                    usermanager, singmanager, db, rolmnger
                );
                var provider = app.ApplicationServices
                    .GetRequiredService<ApplicationPartManager>(); 
                foreach (var part in provider.ApplicationParts)
                { 
                    Console.WriteLine("Loaded part: " + part.Name);
                
                }
                tap = app;
                return tap;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return app;
            }
        }


        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)

        //{
        //    ConfigureSlime(app, env);

        //}
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services = ConfigureServicesSlime(services);
        //}
        public IApplicationBuilder ConfigureRoutdsAndEndpoints(IApplicationBuilder app,
            bool enableExtensions )
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
                        app.UseEndpoints(endpoints =>                        {

                            //var Endpointplugins =
                            SlimePluginManager.LoadEndpointPlugins(extensionsPath,
                                endpoints, app.ApplicationServices);
                            endpoints.MapControllerRoute(
                             name: "default",
                            pattern: pathbase + "/" + "{controller=Home}/{action=Index}/{id?}");

                            endpoints.MapRazorPages();
                            endpoints.MapControllers();

                        });
                        //app.UseMvc(routes =>
                        //{
                        //    routes.MapRoute(name: "default",
                        //    template: pathbase + "/" + "{ controller = Home}/{ action = Index}/{ id?}");
                        //    routes.MapRoute(name: "Post",
                        //    template: pathbase + "/" + "{ controller = Posts}/{action=Index}/{name}/{page?}");
                        //});
                    }
                    else
                    {
                        
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

