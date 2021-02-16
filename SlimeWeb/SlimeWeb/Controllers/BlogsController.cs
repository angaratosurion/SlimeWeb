using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SlimeWeb.Core.App_Start;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;

namespace SlimeWeb.Controllers
{
    public class BlogsController : Controller
    {
       private readonly SlimeDbContext _context;
        private readonly BlogManager blogmnger;// = new BlogManager();
        public BlogsController(SlimeDbContext context)
        {
            _context = context;
            blogmnger = new BlogManager();
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            try
            {

           
            List<ViewBlog> lstblogs = new List<ViewBlog>();
            var list = await blogmnger.ListBlog();
            foreach (var bl in list )
            {
                ViewBlog vb = new ViewBlog();
                vb.ImportFromModel(bl);
                lstblogs.Add(vb);
            }
            return View(lstblogs);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            string name = id;
            if (name == null)
            {
                return NotFound();
            }

            //var blog = await _context.Blogs
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var blog = await blogmnger.GetBlogAsync(name);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        [Authorize]
        public IActionResult Create()
        {


          


           

                    
           
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Title,LastUpdate,Created")] Blog blog)
        {
            //if (ModelState.IsValid)
            {
                //_context.Add(blog);
                //await _context.SaveChangesAsync();
                this.blogmnger.CreateBlog(blog, this.User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            string name = id;
            if (name == null)
            {
                return NotFound();
            }

            // var blog = await _context.Blogs.FindAsync(id);
            var blog = await this.blogmnger.GetBlogAsync(name);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Administrator, Title,Created")] Blog blog)
        {

            string name = id;
            if (name != blog.Name)
            {
                return NotFound();
            }

            // if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(blog);
                    //await _context.SaveChangesAsync();
                    blog.LastUpdate = DateTime.Now;
                    await this.blogmnger.EditBasicInfo(blog, name); ;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await blogmnger.BlogExists(name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            //return View(blog);
        }

        // GET: Blogs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            string name = id;
            if (name == null)
            {
                return NotFound();
            }

            //var blog = await _context.Blogs
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var blog = await this.blogmnger.GetBlogAsync(name);

            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string name = id;
            //var blog = await _context.Blogs.FindAsync(id);
            //_context.Blogs.Remove(blog);
            //await _context.SaveChangesAsync();
            var blog = this.blogmnger.DeleteBlogAsync(name);
            return RedirectToAction(nameof(Index));
        }
    }
}
