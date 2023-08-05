using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlimeWeb.Controllers
{

    // [Authorize(Roles = SlimeWebsUserManager.AdminRoles)]
    [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
  
    public class AdminController : Controller
    {
        AccessManager accessManager;
        //public AdminController(SlimeDbContext context)
        //{
        //    accessManager=new AccessManager(context);
        //}
        public AdminController( )
        {
            accessManager = new AccessManager( );
        }

        // GET: AdminHomeController
        public async Task<ActionResult> IndexAsync()
        {
            //if (await accessManager.DoesUserHasAccess(User.Identity.Name ) == false)
            //{
            //    return RedirectToAction(nameof(Index),"Blogs");
            //}
            return View();
        }

       
    }
}
