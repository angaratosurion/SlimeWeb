using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;

namespace SlimeWeb.Controllers
{
    public class CategoryController : Controller
 {
    //   private readonly SlimeDbContext _context;
        private readonly BlogManager blogmnger;// = new BlogManager();
        private readonly CategoryManager categoryManager;
        private readonly PostManager postManager;
        AccessManager accessManager;

        //public CategoryController(SlimeDbContext context)
        //{
        //    _context = context;
        //    blogmnger = new BlogManager(context);
        //    categoryManager = new CategoryManager(context);
        //    accessManager = new AccessManager(context);
        //    postManager = new PostManager(context);    
        //}

        public CategoryController( )
        {
             
            blogmnger = new BlogManager( );
            categoryManager = new CategoryManager( );
            accessManager = new AccessManager( );
            postManager = new PostManager( );
        }

        // GET: Blogs
        public async Task<IActionResult> Index(string id)
        {
            try
            {
                
           
            List<ViewCategory> lstCategorys = new List<ViewCategory>();
                var list = await categoryManager.ListCategoriesByBlog(id);
                if (list != null)
                {

                    foreach (var bl in list)
                    {
                        ViewCategory vb = new ViewCategory();
                        vb.ImportFromModel(bl);
                        lstCategorys.Add(vb);
                    }
                }
            return View(lstCategorys);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Categorys/Details/5
        public async Task<IActionResult> Details(string  id,string blogname)
        {
            //string name = id;
            //if (name == null)
            //{
            //    return NotFound();
            //}

            ////var Category = await _context.Categorys
            ////    .FirstOrDefaultAsync(m => m.Id == id);
            //var Category = await  categoryManager.GetCategory(id,blogname);
            //if (Category == null)
            //{
            //    return NotFound();
            //}

            //return View(Category);
            return RedirectToAction("ByCategory", "Posts", new { id = blogname, categoryname=id });
        }

        //// GET: Categorys/Create
        //[Authorize]
        //public async Task<IActionResult> Create(string id)
        //{


          

                   
            
                    
           
        //    return View();
        //}

        //// POST: Categorys/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Title,LastUpdate,Created")] Category Category,string blogname)
        //{
        //    //if (ModelState.IsValid)
        //    {
        //        //_context.Add(Category);
        //        //await _context.SaveChangesAsync();
        //        await this. categoryManager.AddNew(Category, blogname);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    //return View(Category);
        //}

        // GET: Categorys/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id,string blogname)
        {
           // string name = blogname;
            

            // var Category = await _context.Categorys.FindAsync(id);
            var Category = await this. categoryManager.GetCategoryById(id);
            if (Category == null)
            {
                return NotFound();
            }
            if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, blogname) == false)
            {
                return RedirectToAction(nameof(Details), new { id = Category.Name, blogname = blogname });
            }
            ViewCategory viewCategory = new ViewCategory();
            viewCategory.ImportFromModel(Category);
            return View(viewCategory);
        }

        // POST: Categorys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category Category,string blogname)
        {

           // string name = id;
            

            // if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(Category);
                    if(blogname == null)
                    {
                       var cat =await categoryManager.GetCategoryById(id);
                        var blog = await blogmnger.GetBlogByIdAsync(cat.BlogId);
                        blogname = blog.Name;
                    }
                   
                await this. categoryManager.Edit(id,Category, blogname); ;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await  categoryManager.Exists(id,blogname))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; 
                    }
                }
                return RedirectToAction(nameof(Index),new { id = blogname });
            }

            //return View(Category);
        }

        // GET: Categorys/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id,string blogname)
        {
           //.. string name = id;
            var Category = await this.categoryManager.GetCategoryById(id);
            //if (id == null)
            //{
            //    return NotFound();
            //}
            if (Category == null)
            {
                return NotFound();
            }
            if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, blogname) == false)
            {
                return RedirectToAction(nameof(Details), new { id = id , blogname =blogname});
            }
          

            //var Category = await _context.Categorys
            //    .FirstOrDefaultAsync(m => m.Id == id);
         
            ViewCategory viewCategory = new ViewCategory();
            viewCategory.ImportFromModel(Category);
            return View(viewCategory);
        }

        // POST: Categorys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id,string blogname)
        {

            //var Category = await _context.Categorys.FindAsync(id);
            //_context.Categorys.Remove(Category);
            //await _context.SaveChangesAsync();
            var cat = await categoryManager.GetCategoryById(id);
            string name = cat.Name;
          var posts= await  postManager.ListPostByCategory(name,blogname);
            if(posts != null)
            {
                foreach(var post in posts)
                {
                    await categoryManager.DetattachCategoryFromPost(post.Id, name, blogname);

                }
            }
            
             this.categoryManager.RemoveCategory(id, blogname);
            return RedirectToAction(nameof(Index),new { id =  blogname });
        }
    }
}
