using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using SlimeWeb.Core.Data.Models;
using System.Threading.Tasks;
using System.Linq;
using SlimeWeb.Core.Managers;

namespace SlimeWeb.Controllers
{
    //[Authorize]
    [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
    public class ClaimsController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IAuthorizationService authService;

        public ClaimsController(UserManager<ApplicationUser> userMgr, IAuthorizationService auth)
        {
            userManager = userMgr;
            authService = auth;
        }

        public IActionResult Index() => View(User?.Claims);

        public IActionResult Create() => View();

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create_Post(string claimType, string claimValue)
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);
            Claim claim = new Claim(claimType, claimValue, ClaimValueTypes.String);
            IdentityResult result = await userManager.AddClaimAsync(user, claim);

            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                Errors(result);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string claimValues)
        {
            ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);

            string[] claimValuesArray = claimValues.Split(";");
            string claimType = claimValuesArray[0], claimValue = claimValuesArray[1], claimIssuer = claimValuesArray[2];

            Claim claim = User.Claims.Where(x => x.Type == claimType && x.Value == claimValue && x.Issuer == claimIssuer).FirstOrDefault();

            IdentityResult result = await userManager.RemoveClaimAsync(user, claim);

            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                Errors(result);

            return View("Index");
        }

        void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public IActionResult Project() => View("Index", User?.Claims);
    
        /* need to modify this */

        [Authorize(Policy = "AllowTom")]
        public ViewResult TomFiles() => View("Index", User?.Claims);

        public async Task<IActionResult> PrivateAccess(string title)
        {
            string adminuser= AppSettingsManager.GetDefaultAdminUserName();
            string[] allowedUsers = { adminuser};//{ "tom", "alice" };
            AuthorizationResult authorized = await authService.AuthorizeAsync(User, allowedUsers, "PrivateAccess");

            if (authorized.Succeeded)
                return View("Index", User?.Claims);
            else
                return new ChallengeResult();
        }
    }
}
