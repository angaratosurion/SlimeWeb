using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;

namespace SlimeWeb.Controllers
{
    [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
    public class BlogModsAdminController : Controller
    {
        //private readonly SlimeDbContext _context;
        BlogModsManager blogModsManager = new BlogModsManager();
        AccessManager accessManager = new AccessManager();
        BlogManager blmngr = new BlogManager();


        public BlogModsAdminController(SlimeDbContext context)
        {
           // _context = context;
        }

        // GET: BlogMods
        public async Task<IActionResult> Index(string id)
        {
            List<ViewBlogMods> viewBlogMods = new List<ViewBlogMods>();
            if (id == null)
            {
                var blogmods = await blogModsManager.ListMods();
                if (blogmods != null)
                {
                    foreach (var bm in blogmods)
                    {
                        ViewBlogMods vblog = new ViewBlogMods();
                        vblog.ImportFromModel(bm);
                        viewBlogMods.Add(vblog);
                    }
                }

            }
            else
            {
                var blogmod = await this.blogModsManager.ListModsByBlogName(id);

                if (blogmod != null)
                {
                    foreach (var bm in blogmod)
                    {
                        ViewBlogMods vblog = new ViewBlogMods();
                        vblog.ImportFromModel(bm);
                        viewBlogMods.Add(vblog);
                    }
                }
            }
            return View(viewBlogMods);
        }

        // GET: BlogMods/Details/5
        public async Task<IActionResult> Details(string id, string moderator)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blogMods = await blogModsManager.Details(id,moderator);
            if (blogMods == null)
            {
                return NotFound();
            }
            ViewBlogMods vblog = new ViewBlogMods();
            vblog.ImportFromModel(blogMods);
             
           

            return View(vblog);
        }
        
        // GET: BlogMods/Create
        public async Task<IActionResult> CreateAsync(string id)
        {
            string blogname = id;

            var blog = await this.blmngr.GetBlogAsync(blogname);


            if (blog != null)
            {

                ViewBag.BlogId = blog.Id;
            }
            return View();
        }

        // POST: BlogMods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Moderator,ModeratorId")] ViewBlogMods blogMods,string id)
        {
            if (ModelState.IsValid)
            {
               
                  this.blogModsManager.RegisterMods(id,(string)User.Identity.Name);
                return RedirectToAction(nameof(Index),new { id = id });
            }
            return View(blogMods);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id,string moderator)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogMods = await blogModsManager.Details( id,moderator);
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
            return View(vblog);
        }

        // POST: BlogMods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("Moderator","Active,ModeratorId")] ViewBlogMods blogMods, 
            string id, 
            string moderator)
        {
            

            if (ModelState.IsValid)
            {
                try
                {
                   var blog=await blmngr.GetBlogAsync(id);
                    if (blog != null)
                    {


                        BlogMods mods = blogMods.ToModel();
                        mods.BlogId= blog.Id;
                       

                        await blogModsManager.Edit(moderator, mods, id);
                    }
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
                return RedirectToAction(nameof(Index),new { id = id });
            }
            return View(blogMods);
        }

        // GET: BlogMods/Delete/5
        public async Task<IActionResult> Delete(string id, string moderator)
        {
            if (id == null)
            {
                return NotFound();
            }


            var blogMods = await this.blogModsManager.Details(id,moderator);
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
        public async Task<IActionResult> DeleteConfirmed(string id, string moderator)
        {
            var blogMods = await this.blogModsManager.Details(id, moderator);
            if (blogMods == null)
            {
                return NotFound();
            }
            ViewBlogMods vblog = new ViewBlogMods();
            vblog.ImportFromModel(blogMods);
            blogModsManager.UnRegisterMods(vblog.Blog.Name, vblog.Moderator);
            return RedirectToAction(nameof(Index));
        }

        //private bool BlogModsExists(int id)
        //{
        //    return _context.BlogMods.Any(e => e.Id == id);
        //}
    }
}
