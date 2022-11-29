using Microsoft.AspNetCore.Mvc;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;

namespace SlimeWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied(string ReturnUrl)
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
        public IActionResult Login(string ReturnUrl)
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
    }
}
