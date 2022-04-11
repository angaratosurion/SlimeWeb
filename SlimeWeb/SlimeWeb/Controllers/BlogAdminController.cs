using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimeWeb.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeWeb.Controllers
{
    public class BlogAdminController : Controller
    {
        AccessManager accessManager = new AccessManager();

        // GET: AdminHomeController
        public async Task<ActionResult> IndexAsync(string id)
        {
            if (await accessManager.DoesUserHasAccess(User.Identity.Name,   id) == false)
            {
                return RedirectToAction(nameof(Index),"Blogs", new { id = id });
            }
            return View();
        }

       
    }
}
