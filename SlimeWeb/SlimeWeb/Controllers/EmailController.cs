using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlimeWeb.Core.Data.Models;
using System.Threading.Tasks;

namespace SlimeWeb.Controllers
{
    public class EmailController : Controller
    {
        
        private UserManager<ApplicationUser> userManager;
        public EmailController(UserManager<ApplicationUser> usrMgr)
        {
            userManager = usrMgr;
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");

            var result = await userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
    }
}
