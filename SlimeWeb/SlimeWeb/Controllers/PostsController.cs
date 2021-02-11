using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;

namespace SlimeWeb
{
    public class PostsController : Controller
    {
        private readonly SlimeDbContext _context;
        PostManager postManager;
        BlogManager blmngr = new BlogManager();
      
         
        public PostsController(SlimeDbContext context)
        {
            _context = context;
            postManager = new PostManager(context);
        }

        // GET: Posts
        public async Task<IActionResult> Index(string id)
        {
            string name = id;

            
            if (name == null)
            {
                return NotFound();
            }
            var p = await postManager.ListByBlogName(name);
            
            List<ViewPost> posts = new List<ViewPost>();
            foreach(var tp in p)
            {
                ViewPost ap = new ViewPost();
                ap.ImportFromModel(tp);
                posts.Add(ap);

            }
            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MarkDownManager markDownManager= new MarkDownManager();
            var mpost = await postManager.Details(id);
            ViewPost post = new ViewPost();
            post.ImportFromModel(mpost);
            post.HTMLcontent = markDownManager.ConvertToHtml(mpost.content);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create(string id)
        {
            string blogname=id;
            var blog = this.blmngr.GetBlogAsync(blogname).Result;
            
            if (blog != null)
            {

                ViewBag.BlogId = blog.Id;
            }
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string blogname,[Bind("Id,Title,Published,content,Author,RowVersion,BlogId,engine")] ViewPost post)
        {
            //if (ModelState.IsValid)
            {
                var mpost = post.ToModel();
              
                 
                await postManager.Create(mpost, this.User.Identity.Name);
                //blmngr.GetBlogAsync()
                return RedirectToAction(nameof(Index),"Blogs",blogname);
            }
           // return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Published,content,Author,RowVersion,BlogId,engine")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
