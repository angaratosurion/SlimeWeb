using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeWeb.Controllers
{
    public class BlogAdminController : Controller
    {

        AccessManager accessManager;
        //public BlogAdminController(SlimeDbContext slimeDbContext) {
        //    accessManager = new AccessManager(slimeDbContext);
        //}
        public BlogAdminController( )
        {
            accessManager = new AccessManager( );
        }
        // GET: AdminHomeController
        public async Task<ActionResult> IndexAsync(string id)
        {
            try
            {
                if (await accessManager.DoesUserHasAccess(User.Identity.Name, id) == false)
                {
                    return RedirectToAction("BlogList", "Blogs", new { id = id });
                }
                return View();
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

       
    }
}
