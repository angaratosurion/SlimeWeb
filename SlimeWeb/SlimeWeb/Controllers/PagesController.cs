﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;
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
        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public async Task<ActionResult> Index()
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

        // GET: PagesController1/Details/5
        public async Task<ActionResult> Details (string name)
        {

            if (name == null)
            {
                return NotFound();
            }

            var mpage = await pageManager.Details(name);
            ViewSlimeWebPage page = new ViewSlimeWebPage();
            page.ImportFromModel(mpage);
            MarkUpManager markUpManager = new MarkUpManager();
            page.HTMLcontent = markUpManager.ConvertToHtml(mpage.content);
            return View(page);
        }
        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public async Task<IActionResult> CreateWithName(string name)
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

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWithName(string name,[Bind("Id,Title,Name,Published,content,Author,RowVersion,engine")] ViewSlimeWebPage page)
        {
            page.Author = CommonTools.usrmng.GetUser(User.Identity.Name);
            page.Name = name;
            var mpage = page.ToModel(page.Author.UserName);
            MarkUpManager markDownManager = new MarkUpManager();
            mpage.content = markDownManager.ConvertFromHtmlToMarkUp(page.content);

            await pageManager.Create(page, User.Identity.Name);

            return RedirectToAction(nameof(Index), "Pages");

        }


        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public async Task<IActionResult> Create()
        {
            SlimeWebPage page=new SlimeWebPage();

            ViewSlimeWebPage viewSlimeWebPage= new ViewSlimeWebPage();
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

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Name,Published,content,Author,RowVersion,engine")] ViewSlimeWebPage page)
        {
            page.Author = CommonTools.usrmng.GetUser(User.Identity.Name);
            var mpage = page.ToModel(page.Author.UserName);
            MarkUpManager markDownManager = new MarkUpManager();
            mpage.content = markDownManager.ConvertFromHtmlToMarkUp(page.content);

           await  pageManager.Create(page, User.Identity.Name);

            return RedirectToAction(nameof(Index), "Pages" );

        }

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public async Task<ActionResult> Edit(string name)
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
            ViewSlimeWebPage vpage= new ViewSlimeWebPage();
            vpage.ImportFromModel(page);
            MarkUpManager markUpManager = new MarkUpManager();
            vpage.HTMLcontent = markUpManager.ConvertToHtml(page.content);
            var cmsengine = AppSettingsManager.GetAppWideCMSEngine();
            if (cmsengine == enumMarkupEngine.QUIL.ToString())
            {
                vpage.content = vpage.content.Replace("{\"ops\":", "");
                vpage.content = vpage.content.Remove(vpage.content.Length - 1, 1);
            }
            return View(vpage);
        }

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string name, [Bind("Id,Title,Published,content,Author,RowVersion,BlogId,engine")] ViewSlimeWebPage page)
        {
            try
            {
                if (name != page.Name)
                {
                    return NotFound();
                }

                var mpage = page.ToModel(User.Identity.Name);
                MarkUpManager markDownManager = new MarkUpManager();
                mpage.content = markDownManager.ConvertFromHtmlToMarkUp(page.content);
                mpage = await pageManager.Edit(name, mpage);

                 
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
            return RedirectToAction(nameof(Index), "Page");


        }

        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        public ActionResult Delete(int id)
        {
            return View();
        }
        [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
        // POST: PagesController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index ));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> UploadQuill(/*[FromQuery] string bid*/)//,[FromBody]IFormFile file)
        {
            try
            {

                // string Blogid = bid;
                var pages = await pageManager.List();
                int pid = -1;
                pid = await fileRecordManager.PredictLastId("Pages") + 1;
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
                return null;
            }
        }
    }
}
