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
    public class TagController : Controller
    {
       private readonly SlimeDbContext _context;
        private readonly BlogManager blogmnger;// = new BlogManager();
        private readonly TagManager TagManager;
        private readonly PostManager postManager;
        AccessManager accessManager = new AccessManager();

        public TagController(SlimeDbContext context)
        {
            _context = context;
            blogmnger = new BlogManager();
            TagManager = new TagManager();
            postManager = new PostManager();    
        }

        // GET: Blogs
        public async Task<IActionResult> Index(string id)
        {
            try
            {
                
           
            List<ViewTag> lstTags = new List<ViewTag>();
                var list = await TagManager.GetTagsByBlog(id);

            foreach (var bl in list )
            {
                ViewTag vb = new ViewTag();
                vb.ImportFromModel(bl);
                lstTags.Add(vb);
            }
            return View(lstTags);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(string  id,string blogname)
        {
            //string name = id;
            //if (name == null)
            //{
            //    return NotFound();
            //}

            ////var Tag = await _context.Tags
            ////    .FirstOrDefaultAsync(m => m.Id == id);
            //var Tag = await  TagManager.GetTag(id,blogname);
            //if (Tag == null)
            //{
            //    return NotFound();
            //}

            //return View(Tag);
            return RedirectToAction("ByTag", "Posts", new { id = blogname, Tagname=id });
        }

        //// GET: Tags/Create
        //[Authorize]
        //public async Task<IActionResult> Create(string id)
        //{


          

                   
            
                    
           
        //    return View();
        //}

        //// POST: Tags/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Title,LastUpdate,Created")] Tag Tag,string blogname)
        //{
        //    //if (ModelState.IsValid)
        //    {
        //        //_context.Add(Tag);
        //        //await _context.SaveChangesAsync();
        //        await this. TagManager.AddNew(Tag, blogname);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    //return View(Tag);
        //}

        // GET: Tags/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id,string blogname)
        {
           // string name = blogname;
            

            // var Tag = await _context.Tags.FindAsync(id);
            var tag = await this. TagManager.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, blogname) == false)
            {
                return RedirectToAction(nameof(Details), new { id = tag.Name, blogname = blogname });
            }
            ViewTag viewTag = new ViewTag();
            viewTag.ImportFromModel(tag);
            return View(viewTag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Tag tag,string blogname)
        {

           // string name = id;
            

            // if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(Tag);
                    if(blogname == null)
                    {
                       var cat =await TagManager.GetTagById(id);
                        var blog = await blogmnger.GetBlogByIdAsync(cat.BlogId);
                        blogname = blog.Name;
                    }
                   
                await this. TagManager.Edit(id,tag, blogname); ;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await  TagManager.Exists(id,blogname))
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

            //return View(Tag);
        }

        // GET: Tags/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id,string blogname)
        {
           //.. string name = id;
            var Tag = await this.TagManager.GetTagById(id);
            //if (id == null)
            //{
            //    return NotFound();
            //}
            if (Tag == null)
            {
                return NotFound();
            }
            if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, blogname) == false)
            {
                return RedirectToAction(nameof(Details), new { id = id , blogname =blogname});
            }
          

            //var Tag = await _context.Tags
            //    .FirstOrDefaultAsync(m => m.Id == id);
         

            return View(Tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id,string blogname)
        {
            string name = id;
            //var Tag = await _context.Tags.FindAsync(id);
            //_context.Tags.Remove(Tag);
            //await _context.SaveChangesAsync();
          var posts= await  postManager.ListPostByTag(name,blogname);
            if(posts != null)
            {
                foreach(var post in posts)
                {
                    await TagManager.DetattachTagFromPost(post.Id, id, blogname);

                }
            }
            
             this.TagManager.RemoveTag(name, blogname);
            return RedirectToAction(nameof(Index),new { id =  blogname });
        }
    }
}
