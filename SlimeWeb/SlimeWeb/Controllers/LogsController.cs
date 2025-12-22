using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimeWeb.Core.Data.NonDataModels;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;

namespace SlimeWeb.Controllers
{
    public class LogsController : Controller
    {
        SlimeLogManager slimeLogManager = new SlimeLogManager();
        // GET: LogController
        public ActionResult Index()
        {
            try
            {
                var logs = this.slimeLogManager.List();
                
                
                //if (logs==null)
                //{
                //    return NotFound();
                //}
                return View(logs);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }
        public PartialViewResult OnGetLogPartial(string level)
        {
            try
            {
                List<ExceptionModel> Logs = slimeLogManager.ListByLevel(level);
               
                if (Logs == null)
                {
                    return PartialView("LogView_Partial");
                }
                Logs = slimeLogManager.ListByLevel(level);

                return PartialView("LogView_Partial", Logs);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }
        public PartialViewResult OnGetLogListPartial( )
        {
            try
            {
                List<ExceptionModel> Logs = slimeLogManager.List();
                 
                if (Logs == null)
                {
                    return PartialView("LogView_Partial");
                }
                

                return PartialView("LogView_Partial", Logs);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return null;
            }
        }

        // GET: LogController/Details/5
        public ActionResult Details(string TimeStamp)
        {
            try
            {
                var list = slimeLogManager.Details(TimeStamp);
                if(TimeStamp==null && list==null)
                {
                    return NotFound();
                }

                return View(list);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }



        // GET: LogController/Delete/5
        public ActionResult Delete( )
        {
            return View();
        }

        // POST: LogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost( )
        {
            try
            {
                slimeLogManager.DeleteLog();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        public ActionResult DeleteArchive()
        {
            return View();
        }

        // POST: LogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteArchivePost()
        {
            try
            {
                slimeLogManager.DeleteLogArchive();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
