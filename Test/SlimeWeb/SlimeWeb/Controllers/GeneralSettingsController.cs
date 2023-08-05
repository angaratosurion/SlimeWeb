using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Managers.Markups;
using SlimeWeb.Core.Tools;
using System.Threading.Tasks;
using System;
using SlimeWeb.Core.Data.Models;

namespace SlimeWeb.Controllers
{
    [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]

    public class GeneralSettingsController : Controller
    {
        GeneralSettingsManager generalSettingsManager= new GeneralSettingsManager();
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            try
            {


                ViewGeneralSettings model = this.generalSettingsManager.Details();
                if (model == null)
                {
                    return NotFound();
                }



                
                return View(model);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( ViewGeneralSettings model)
        {
            try
            {


                // if (ModelState.IsValid)
                //  {

                //_context.Update(post);
                //await _context.SaveChangesAsync();
                generalSettingsManager.Edit(model.ToModel()) ;
                // }
                return RedirectToAction(nameof(Index), "Admin");

            }

            catch (DbUpdateConcurrencyException)
            {

                return null;
                //if (this.postManager.Exists(post.Id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }




            //return View(post);
        }


    }
}
