using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Managers.Markups;
using SlimeWeb.Core.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlimeWeb.Controllers
{
    public class PagesController : Controller
    { 
        SlimeWebPageManager pageManager = new SlimeWebPageManager();
        FileRecordManager fileRecordManager= new FileRecordManager();
        GeneralSettingsManager generalSettingsManager = new GeneralSettingsManager();
        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public async Task<ActionResult> Index()
        {
            try
            {
                var p = await pageManager.List();

                List<ViewSlimeWebPage> pages = new List<ViewSlimeWebPage>();
                if (p != null)
                {
                    foreach (var tp in p)
                    {
                        ViewSlimeWebPage ap = new ViewSlimeWebPage();
                        ap.ImportFromModel(tp);
                        pages.Add(ap);

                    }
                }
                return View(pages);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<IActionResult> IndexPaged( int page)
        {

            try
            {
                ViewData["Title"] = "Pages";

                //string name = id;
                //if (name == null)
                //{
                //    return NotFound();
                //}
                //ViewBag.Name = name;
                int pagesize = 0;

                var gensettings = generalSettingsManager.Details();
                pagesize = gensettings.ItemsPerPage;
                int count = (await pageManager.List()).Count;
                if (pagesize <= 0)
                {
                    pagesize = count;
                }


                List<SlimeWebPage> p = await pageManager.ListByPublished( page, pagesize);
                this.ViewBag.MaxPage = (count / pagesize) - (count % pagesize == 0 ? 1 : 0);
                List<ViewSlimeWebPage> SlimeWebPages = new List<ViewSlimeWebPage>();
                if (p != null)
                {
                    this.ViewBag.Page = page;


                    if (p != null)
                    {
                        foreach (var tp in p)
                        {
                            ViewSlimeWebPage ap = new ViewSlimeWebPage();
                            ap.ImportFromModel(tp);
                            SlimeWebPages.Add(ap);

                        }
                    }
                }
                return View(SlimeWebPages);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        // GET: PagesController1/Details/5
        public async Task<ActionResult> Details (string name)
        {
            try
            {
                if (name == null)
                {
                    return NotFound();
                }

                var mpage = await pageManager.Details(name);
                ViewSlimeWebPage page = new ViewSlimeWebPage();
                page.ImportFromModel(mpage);
                
                page.HTMLcontent = MarkUpManager.ConvertToHtml(mpage.content);
                return View(page);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        [AllowAnonymous]
        public async Task<ActionResult> View(string name)
        {

            try
            {
                SlimeWebPageManager pageManager = new SlimeWebPageManager();
                SlimeWebPage page = pageManager.Details(name).Result;
                if (page == null)
                {
                    return RedirectToAction("CreateWithName", "Pages", new { name = name });
                }
                else
                {
                    ViewSlimeWebPage vpage = new ViewSlimeWebPage();
                   // MarkUpManager MarkUpManager = new MarkUpManager();
                    vpage.ImportFromModel(page);
                    vpage.HTMLcontent = MarkUpManager.ConvertToHtml(vpage.content);
                    return View(vpage);

                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }
        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public async Task<IActionResult> CreateWithName(string name)
        {
            try
            {
                SlimeWebPage pages = new SlimeWebPage();
                ViewSlimeWebPage viewSlimeWebPage = new ViewSlimeWebPage();
                pages.Name = name;
                pages.Author = CommonTools.usrmng.GetUser(User.Identity.Name).UserName;
                viewSlimeWebPage.ImportFromModel(pages);
                ViewBag.CreateAction = true;
                var blpath = FileSystemManager.GetPagesRootDataFolderAbsolutePath(name);
                //   if (FileSystemManager.DirectoryExists(blpath) == false)
                {
                    FileSystemManager.CreateDirectory(blpath);

                    //string pathbase;
                    string pathbase = AppSettingsManager.GetPathBase();
                    if (CommonTools.isEmpty(pathbase) == false)
                    {
                        ViewBag.pathbase = pathbase;
                    }

                }
                return View(viewSlimeWebPage);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWithName(string name,[Bind("Id,Title,Name,Published,content,Author,RowVersion,engine")] ViewSlimeWebPage page)
        {
            try
            {
                page.Author = CommonTools.usrmng.GetUser(User.Identity.Name);
                page.Name = name;
                var mpage = page.ToModel(page.Author.UserName);
                //MarkUpManager MarkUpManager = new MarkUpManager();
                mpage.content = MarkUpManager.ConvertFromHtmlToMarkUp(page.content);

                await pageManager.Create(page, User.Identity.Name);

                return RedirectToAction(nameof(Index), "Pages");
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }


        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public async Task<IActionResult> Create()
        {
            try
            {
                SlimeWebPage page = new SlimeWebPage();

                ViewSlimeWebPage viewSlimeWebPage = new ViewSlimeWebPage();
                page.Author = CommonTools.usrmng.GetUser(User.Identity.Name).UserName;
                viewSlimeWebPage.ImportFromModel(page);
                ViewBag.CreateAction = true;


                //string pathbase;
                string pathbase = AppSettingsManager.GetPathBase();
                if (CommonTools.isEmpty(pathbase) == false)
                {
                    ViewBag.pathbase = pathbase;
                }
                return View(viewSlimeWebPage);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Name,Published,content,Author,RowVersion,engine")] ViewSlimeWebPage page)
        {
            try

            {
                page.Author = CommonTools.usrmng.GetUser(User.Identity.Name);
                var mpage = page.ToModel(page.Author.UserName);
                //MarkUpManager MarkUpManager = new MarkUpManager();
                mpage.content = MarkUpManager.ConvertFromHtmlToMarkUp(page.content);

                await pageManager.Create(page, User.Identity.Name);

                return RedirectToAction(nameof(Index), "Pages");
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public async Task<ActionResult> Edit(string name)
        {
            try
            {
                ViewBag.CreateAction = false;



                //string pathbase;
                string pathbase = AppSettingsManager.GetPathBase();
            if (CommonTools.isEmpty(pathbase) == false)
            {
                ViewBag.pathbase = pathbase;
            }

            if (name == null)
            {
                return NotFound();
            }
            var page = await pageManager.Details(name);
            if (page == null)
            {
                return NotFound();
            }
            ViewSlimeWebPage vpage = new ViewSlimeWebPage();
            vpage.ImportFromModel(page);
           // MarkUpManager MarkUpManager = new MarkUpManager();
            vpage.HTMLcontent = MarkUpManager.ConvertToHtml(page.content);
            var cmsengine = AppSettingsManager.GetAppWideCMSEngine();
            if (cmsengine == enumMarkupEngine.QUIL.ToString())
            {
                vpage.content = vpage.content.Replace("{\"ops\":", "");
                vpage.content = vpage.content.Remove(vpage.content.Length - 1, 1);
            }
            return View(vpage);
        }
        catch (Exception ex) { CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string Name, [Bind("Id,Title,Name,Published,content,Author,RowVersion,engine,TopPosition,BottomPosition")] ViewSlimeWebPage page)
        {
            try
            {
                if (Name != page.Name)
                {
                    return NotFound();
                }

                var mpage = page.ToModel(User.Identity.Name);
                //MarkUpManager MarkUpManager = new MarkUpManager();
                mpage.content = MarkUpManager.ConvertFromHtmlToMarkUp(page.content);
                mpage = await pageManager.Edit( Name, mpage);
                return RedirectToAction(nameof(Index), "Pages");


            }
            catch (DbUpdateConcurrencyException)
            {


                if (await this.pageManager.Exists(page.Name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
           


        }

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public async Task<ActionResult> DeleteAsync( string Name)
        {
            try
            {

                if (Name == null)
                {
                    return NotFound();
                }
                var mpage = await pageManager.Details(Name);
                ViewSlimeWebPage page = new ViewSlimeWebPage();
                page.ImportFromModel(mpage);
               // MarkUpManager MarkUpManager = new MarkUpManager();
                page.HTMLcontent = MarkUpManager.ConvertToHtml(mpage.content);
                if (page == null)
                {
                    return NotFound();
                }



                return View(page);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        // SlimeWebPage: PagesController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string Name )
        {
            try
            {

                await this.pageManager.Delete(Name);
                return RedirectToAction(nameof(Index ));
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult> UploadQuill(/*[FromQuery] string bid*/)//,[FromBody]IFormFile file)
        {
            try
            {

                // string Blogid = bid;
               // var pages = await pageManager.List();
                int pid = -1;
                pid = await IDataManager.PredictLastId("Pages") + 1;
                if (Request.Form.Files.Count == 0)
                {
                    return NoContent();
                }
                string bid = null;
                IFormFile formFile = (FormFile)Request.Form.Files[0];

                if (bid == null)
                {
                    bid = Request.RouteValues["id"].ToString();
                }
               var  pages = await pageManager.Details(bid);
                if(pages!=null)
                {
                    pid = pages.Id;
                }




                var path = await fileRecordManager.CreateForPage(pid,  new Files(), formFile, User.Identity.Name);





                // return Content(Url.Content(@"~\Uploads\" + fileid));
                //return Content(path);
                //return Json(new { location = this.HttpContext.Request.Host+"/"+path });
                //return Json(new { location = "/" + path });

                return Content("/" + path);
                // return null;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
