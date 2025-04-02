using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;

namespace SlimeWeb.Controllers
{
    public class BlogsController : Controller
    {
       //private readonly SlimeDbContext _context;
        private readonly BlogManager blogmnger;// = new BlogManager();
        AccessManager accessManager;
        //public BlogsController(SlimeDbContext context)
        //{
        //    _context = context;
        //    blogmnger = new BlogManager(context);
        //    accessManager = new AccessManager(context);
        //}
        public BlogsController( )
        {
           
            blogmnger = new BlogManager( );
            accessManager = new AccessManager( );
        }
        private readonly ILogger<HomeController> _logger;

        public BlogsController(ILogger<HomeController> logger)
        {
            _logger = logger;
            blogmnger = new BlogManager();
            accessManager = new AccessManager();
        }


        public async Task<IActionResult> Index()
        {
            try
            {


                List<ViewBlog> lstblogs = new List<ViewBlog>();
                var list = await blogmnger.ListBlogBylastUpdated();
                if (list != null)
                {
                    foreach (var bl in list)
                    {
                        ViewBlog vb = new ViewBlog();
                        vb.ImportFromModel(bl);
                        lstblogs.Add(vb);
                    }
                }
                return View(lstblogs);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);



                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            }
        }
        public async Task<IActionResult> BlogList()
        {
            try
            {


                List<ViewBlog> lstblogs = new List<ViewBlog>();
                var list = await blogmnger.ListBlog();
                if (list != null)
                {
                    foreach (var bl in list)
                    {
                        ViewBlog vb = new ViewBlog();
                        vb.ImportFromModel(bl);
                        lstblogs.Add(vb);
                    }
                }
                return View(lstblogs);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);



                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

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
        public async Task<IActionResult> Create()
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
            try
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
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: Blogs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                string name = id;
                if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, id) == false)
                {
                    return RedirectToAction(nameof(Details), new { id = id });
                }
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
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Administrator, Title,Created")] Blog blog)
        {
            try
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
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            //return View(blog);
        }

        // GET: Blogs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                string name = id;
                if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, name) == false)
                {
                    return RedirectToAction(nameof(Details), new { id = name });
                }
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
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                string name = id;
                //var blog = await _context.Blogs.FindAsync(id);
                //_context.Blogs.Remove(blog);
                //await _context.SaveChangesAsync();
                var blog = this.blogmnger.DeleteBlogAsync(name);
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
