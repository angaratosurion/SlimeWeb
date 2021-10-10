using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;

namespace SlimeWeb.Controllers
{
    public class BlogModsController : Controller
    {
        //private readonly SlimeDbContext _context;
        BlogModsManager blogModsManager = new BlogModsManager();
        AccessManager accessManager = new AccessManager();


        public BlogModsController(SlimeDbContext context)
        {
           // _context = context;
        }

        // GET: BlogMods
        public async Task<IActionResult> Index(string blogname)
        {
            List<ViewBlogMods> viewBlogMods = new List<ViewBlogMods>();
            var blogmod = await this.blogModsManager.ListModsByBlogName(blogname);
            if(blogmod==null)
            {
                return NotFound();

            }

           foreach(var bm in blogmod)
            {
                ViewBlogMods vblog = new ViewBlogMods();
                vblog.ImportFromModel(bm);
                viewBlogMods.Add(vblog);
            }
            
            return View(viewBlogMods);
        }

        // GET: BlogMods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blogMods = await blogModsManager.Details((int)id);
            if (blogMods == null)
            {
                return NotFound();
            }
            ViewBlogMods vblog = new ViewBlogMods();
            vblog.ImportFromModel(blogMods);
             
           

            return View(vblog);
        }

        // GET: BlogMods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogMods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Moderator")] ViewBlogMods blogMods)
        {
            if (ModelState.IsValid)
            {
               
                  this.blogModsManager.RegisterMods(blogMods.Blog.Name,(string)User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(blogMods);
        }

        // GET: BlogMods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogMods = await blogModsManager.Details((int) id);
            if (blogMods == null)
            {
                return NotFound();
            }
            ViewBlogMods vblog = new ViewBlogMods();
            vblog.ImportFromModel(blogMods);
            if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, vblog.Blog.Name) == false)
            {
                return RedirectToAction(nameof(Index), "BlogMods", new { id = id });
            }
            return View(blogMods);
        }

        // POST: BlogMods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Moderator")] BlogMods blogMods)
        {
            if (id != blogMods.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(blogMods);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!BlogModsExists(blogMods.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogMods);
        }

        // GET: BlogMods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var blogMods = await this.blogModsManager.Details((int)id);
            if (blogMods == null)
            {
                return NotFound();
            }
            ViewBlogMods vblog = new ViewBlogMods();
            vblog.ImportFromModel(blogMods);
            if (await this.accessManager.DoesUserHasAccess(User.Identity.Name,vblog.Blog.Name) == false)
            {
                return RedirectToAction(nameof(Index), "BlogMods", new { id = id });
            }


            return View(vblog);
        }

        // POST: BlogMods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogMods = await this.blogModsManager.Details((int)id);
            if (blogMods == null)
            {
                return NotFound();
            }
            ViewBlogMods vblog = new ViewBlogMods();
            vblog.ImportFromModel(blogMods);
            blogModsManager.UnRegisterMods(vblog.Blog.Name, vblog.Moderator.UserName);
            return RedirectToAction(nameof(Index));
        }

        //private bool BlogModsExists(int id)
        //{
        //    return _context.BlogMods.Any(e => e.Id == id);
        //}
    }
}
