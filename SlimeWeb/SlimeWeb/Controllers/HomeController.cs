using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Managers.Markups;
using SlimeWeb.Core.Tools;
using SlimeWeb.Models;
using System;
using System.Diagnostics;

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
            try
            {
                string defaultcon = "", defautaction = "", pathbase = "";


                //return View();
                bool hostedinsubfolder = false;

                 

                //

                defaultcon = AppSettingsManager.GetDefaultController();
                pathbase = AppSettingsManager.GetPathBase();
                hostedinsubfolder = AppSettingsManager.GetHostedInSubFolderSetting();


                if (CommonTools.isEmpty(defaultcon) == false && hostedinsubfolder
                    && CommonTools.isEmpty(pathbase) == false)
                {
                    Response.Redirect(pathbase + "/" + defaultcon);
                }
                else
                {
                    Response.Redirect(defaultcon);
                }

                return View();

                //return View();
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }
        public IActionResult AboutSoftware()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

            public IActionResult Privacy()
        {
            try
            {
                SlimeWebPageManager pageManager = new SlimeWebPageManager();
                SlimeWebPage page = pageManager.Details("Privacy").Result;
                if (page == null)
                {
                    return RedirectToAction("CreateWithName", "Pages", new { name = "Privacy" });
                }
                else
                {
                    ViewSlimeWebPage vpage = new ViewSlimeWebPage();
                    MarkUpManager markUpManager = new MarkUpManager();
                    vpage.ImportFromModel(page);
                    vpage.HTMLcontent = markUpManager.ConvertToHtml(vpage.content);
                    return View(vpage);

                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
