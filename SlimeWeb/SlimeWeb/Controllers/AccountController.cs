using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;

namespace SlimeWeb.Controllers
{
   // [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
    public class AccountController : Controller
    {
        public IActionResult AccessDenied(string ReturnUrl)
        {
            try
            { 
                var pathbase = AppSettingsManager.GetPathBase();

                if (CommonTools.isEmpty(pathbase) == false)
                {
                    return LocalRedirect(pathbase + "/Identity/Account/AccessDenied?ReturnUrl=" + ReturnUrl);
                    // return LocalRedirect("/Identity/Account/AccessDenied");
                }
                else
                {
                    return LocalRedirect("/Identity/Account/AccessDenied?ReturnUrl=" + ReturnUrl);
                }
            }
            catch (Exception ex) { CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
           
        }
        public IActionResult Login(string ReturnUrl)
        {
            try
            {
                var pathbase = AppSettingsManager.GetPathBase();

                if (CommonTools.isEmpty(pathbase) == false)
                {

                    return LocalRedirect(pathbase + "/Identity/Account/Login?ReturnUrl=" + ReturnUrl);


                }
                else
                {
                    return LocalRedirect("/Identity/Account/Login?ReturnUrl=" + ReturnUrl);
                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }


        }
    }
}
