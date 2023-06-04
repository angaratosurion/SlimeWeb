using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using SimpleSystemSpecs.Core;
using SimpleSystemSpecs.Core.Data.Models;
using SlimeWeb.Core.Managers;
using System.IO;

namespace SlimeWeb.Controllers
{
    public class HWSpecs : Controller
    {
        public IActionResult Index()
        {
            
            SystemSpecs [] spec =new  SystemSpecs[3];
            SystemSpecsManager manager = new SystemSpecsManager();
            spec[0] = manager.GetSpecs(false);
            string appdir = FileSystemManager.GetAppRootFolderAbsolutePath();
            var tspec=manager.LoadSpecs(Path.Combine(appdir, "desktop.spec")); ;
            spec[1] = tspec;

            return View(spec);
        }
    }
}
