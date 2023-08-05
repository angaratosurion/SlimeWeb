using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlimeWeb.Core.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using SlimeWeb.Core.Data.NonDataModels.UserAndRoles;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SlimeWeb.Core.Tools;
using System;

namespace SlimeWeb.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class RoleController : Controller
    {
        private RoleManager<ApplicationRole> roleManager;
        private UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<ApplicationRole> roleMgr, UserManager<ApplicationUser> userMrg)
        {
            roleManager = roleMgr;
            userManager = userMrg;
        }

        public IActionResult Index() => View(roleManager.Roles);

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await roleManager.CreateAsync(new ApplicationRole(name));
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
                return View(name);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> Update(string id)
        {try
            {
                IdentityRole role = await roleManager.FindByIdAsync(id);
                List<ApplicationUser> members = new List<ApplicationUser>();
                List<ApplicationUser> nonMembers = new List<ApplicationUser>();
                foreach (ApplicationUser user in userManager.Users)
                {
                    var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                    list.Add(user);
                }
                return View(new RoleEdit
                {
                    Role = role,
                    Members = members,
                    NonMembers = nonMembers
                });
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            try
            {
                IdentityResult result;
                if (ModelState.IsValid)
                {
                    foreach (string userId in model.AddIds ?? new string[] { })
                    {
                        ApplicationUser user = await userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            result = await userManager.AddToRoleAsync(user, model.RoleName);
                            if (!result.Succeeded)
                                Errors(result);
                        }
                    }
                    foreach (string userId in model.DeleteIds ?? new string[] { })
                    {
                        ApplicationUser user = await userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                            if (!result.Succeeded)
                                Errors(result);
                        }
                    }
                }

                if (ModelState.IsValid)
                    return RedirectToAction(nameof(Index));
                else
                    return await Update(model.RoleId);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                ApplicationRole role = await roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    IdentityResult result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
                else
                    ModelState.AddModelError("", "No role found");
                return View("Index", roleManager.Roles);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
