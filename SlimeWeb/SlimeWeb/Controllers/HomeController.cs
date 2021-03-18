using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SlimeWeb.Core;
using SlimeWeb.Models;

namespace SlimeWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string defaultcon="", defautaction="",pathbase="";
            string pathwithextention = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string path = System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\", "");
            //return View();
            bool hostedinsubfolder = false;
            var builder = new ConfigurationBuilder()
                            .SetBasePath(path)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var config = builder.Build();//

            defaultcon = config.GetValue<string>("ApppSettings:DefaultRoot:Controller");
            pathbase = config.GetValue<string>("ApppSettings:PathBase");
            hostedinsubfolder = config.GetValue<bool>("ApppSettings:HostedInSubFolder");

            if (CommonTools.isEmpty(defaultcon) == false && hostedinsubfolder 
                && CommonTools.isEmpty(pathbase) == false)
            {
                Response.Redirect(pathbase+"/"+ defaultcon );
            }
                 else
            {
                Response.Redirect(defaultcon);
            }
              
                return View();
            
            //return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
