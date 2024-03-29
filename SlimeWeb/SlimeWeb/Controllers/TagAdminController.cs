﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReverseMarkdown.Converters;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Tools;

namespace SlimeWeb.Controllers
{
    [Authorize(Policy = SlimeWebsUserManager.AdminRoles)]
    public class TagAdminController : Controller
    {
       private readonly SlimeDbContext _context;
        private readonly BlogManager blogmnger;// = new BlogManager();
        private readonly TagManager TagManager;
        private readonly PostManager postManager;
        AccessManager accessManager;

        //public TagAdminController(SlimeDbContext context)
        //{
        //    _context = context;
        //    blogmnger = new BlogManager(context);
        //    TagManager = new TagManager(context);
        //    postManager = new PostManager(context);
        //    accessManager = new AccessManager(context);
        //}
        public TagAdminController( )
        {
            
            blogmnger = new BlogManager( );
            TagManager = new TagManager( );
            postManager = new PostManager( );
            accessManager = new AccessManager( );
        }
        // GET: Blogs
        public async Task<IActionResult> Index(string id)
        {
            try
            {
                
           
            List<ViewTag> lstTags = new List<ViewTag>();


                if (id == null)
                {
                    var list = await TagManager.ListTags();
                    if (list != null)
                    {

                        foreach (var bl in list)
                        {
                            ViewTag vb = new ViewTag();
                            vb.ImportFromModel(bl);
                            lstTags.Add(vb);
                        }
                    }

                }
                else
                {
                    var list = await TagManager.GetTagsByBlog(id);
                    if (list != null)
                    {
                        foreach (var bl in list)
                        {
                            ViewTag vb = new ViewTag();
                            vb.ImportFromModel(bl);
                            lstTags.Add(vb);
                        }
                    }
                }
            return View(lstTags);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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
            try
            {

                // var Tag = await _context.Tags.FindAsync(id);
                var tag = await this.TagManager.GetTagById(id);
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
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Tag tag,string blogname)
        {
            try
            {
                // string name = id;


                // if (ModelState.IsValid)
                {
                    try
                    {
                        //_context.Update(Tag);
                        if (blogname == null)
                        {
                            var cat = await TagManager.GetTagById(id);
                            var blog = await blogmnger.GetBlogByIdAsync(cat.BlogId);
                            blogname = blog.Name;
                        }

                        await this.TagManager.Edit(id, tag, blogname); ;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await TagManager.Exists(id, blogname))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index), new { id = blogname });
                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

            //return View(Tag);
        

        // GET: Tags/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id,string blogname)
        {
            try
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
                    return RedirectToAction(nameof(Details), new { id = id, blogname = blogname });
                }


                //var Tag = await _context.Tags
                //    .FirstOrDefaultAsync(m => m.Id == id);


                return View(Tag);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id,string blogname)
        {
            try
            {
                string name = id;
                //var Tag = await _context.Tags.FindAsync(id);
                //_context.Tags.Remove(Tag);
                //await _context.SaveChangesAsync();
                var posts = await postManager.ListPostByTag(name, blogname);
                if (posts != null)
                {
                    foreach (var post in posts)
                    {
                        await TagManager.DetattachTagFromPost(post.Id, id, blogname);

                    }
                }

                this.TagManager.RemoveTag(name, blogname);
                return RedirectToAction(nameof(Index), new { id = blogname });
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
