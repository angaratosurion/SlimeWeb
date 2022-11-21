using Microsoft.AspNetCore.Mvc;

namespace SlimeWeb.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {

            return LocalRedirect("/Identity/Account/AccessDenied");
             
        }
    }
}
