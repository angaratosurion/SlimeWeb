using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SlimeWeb.Core;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.Models.Interfaces;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;
using SlimeWeb.Core.Managers.Interfaces;
using SlimeWeb.Core.Managers.Managment;
using SlimeWeb.Core.Managers.Markups;
using SlimeWeb.Core.Tools;

namespace SlimeWeb.Controllers
{
    public class PostsController : Controller
    {
        private readonly SlimeDbContext _context;
         PostManager postManager;
        BlogManager blmngr;
         CategoryManager categoryManager;
        TagManager TagManager;
        AccessManager accessManager;
        FileRecordManager fileRecordManager;
        GeneralSettingsManager generalSettingsManager;
        
        public PostsController()
        {
            if (AppSettingsManager.GetAllowChangingManagers())
            {
                GroupedManagers groupedManagers = ManagerManagment.GetDefaultManagger();
                if (groupedManagers != null)
                {
                    if (groupedManagers.AccessManager != null)
                    {
                        accessManager = groupedManagers.AccessManager;

                    }
                    else { accessManager = new AccessManager(); }
                    if (groupedManagers.PostManager != null)
                    {
                        postManager = groupedManagers.PostManager;
                    }
                    else
                    {
                        postManager = new PostManager();//as IPostManager<IPost>;

                    }
                    if (groupedManagers.CategoryManager != null)
                    {
                        categoryManager = groupedManagers.CategoryManager;
                    }
                    else
                    {

                        categoryManager = new CategoryManager();
                    }
                    if (groupedManagers.FileManager != null)
                    {
                        fileRecordManager = groupedManagers.FileManager;
                    }
                    else
                    {
                        fileRecordManager = new FileRecordManager();

                    }
                    generalSettingsManager = new GeneralSettingsManager();


                }
            }
            else
            {
                postManager = new PostManager();
                categoryManager = new CategoryManager();
                         
                accessManager = new AccessManager();
                blmngr = new BlogManager();
                TagManager = new TagManager();
                fileRecordManager = new FileRecordManager();
                      
                generalSettingsManager = new GeneralSettingsManager();
            }
            
        }


         

        // GET: Posts
        public async Task<IActionResult> Index(string id)
        {

            try
            {
                string name = id;


                if (name == null)
                {
                    return NotFound();
                }
                var p = await postManager.ListByBlogNameByPublished(name);

                List<ViewPost> posts = new List<ViewPost>();
                if (p != null)
                {
                    foreach (var tp in p)
                    {
                        ViewPost ap = new ViewPost();
                        ap.ImportFromModel((Post)tp);
                        posts.Add(ap);

                    }
                }
                return View(posts);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
       // [Route("{controller}/{action}/{name}/{page}")]
        public async Task<IActionResult> IndexPaged(string id, int page)
        {

            try
            {
                ViewData["Title"] = "Posts";

                string name = id;
                if (name == null)
                {
                    return NotFound();
                }
                ViewBag.Name = name;
                int pagesize = 0;

                var gensettings = generalSettingsManager.Details();
                pagesize = gensettings.ItemsPerPage;
                int count = (await postManager.ListByBlogName(name)).Count;
                if (pagesize <= 0)
                {
                    pagesize = count;
                }


                List<Post> p = await postManager.ListByBlogNameByPublished(name, page, pagesize);
                this.ViewBag.MaxPage = (count / pagesize) - (count % pagesize == 0 ? 1 : 0);
                List<ViewPost> posts = new List<ViewPost>();
                if (p != null)
                {
                    this.ViewBag.Page = page;


                    if (p != null)
                    {
                        foreach (var tp in p)
                        {
                            ViewPost ap = new ViewPost();
                            ap.ImportFromModel(tp);
                            posts.Add(ap);

                        }
                    }
                }
                return View(posts);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
       
        public async Task<IActionResult> ByCategory(string id,string categoryname)
        {
            try
            {
                string name = id;


                if (name == null && categoryname != null)
                {
                    return NotFound();
                }
                // var p = await postManager.ListByBlogNameByPublished(name);
                var p = await postManager.ListPostByCategory(categoryname, name);
                p.OrderByDescending(x => x.Published);
                List<ViewPost> posts = new List<ViewPost>();
                if (p != null)
                {
                    foreach (var tp in p)
                    {
                        ViewPost ap = new ViewPost();
                        ap.ImportFromModel(tp);
                        posts.Add(ap);

                    }
                }
                return View(posts);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<IActionResult> ByTag(string id, string tagname)
        {
            try
            {
                string name = id;


                if (name == null && tagname != null)
                {
                    return NotFound();
                }
                // var p = await postManager.ListByBlogNameByPublished(name);
                var p = await postManager.ListPostByTag(tagname, name);
                p.OrderByDescending(x => x.Published);
                List<ViewPost> posts = new List<ViewPost>();
                if (p != null)
                {
                    foreach (var tp in p)
                    {
                        ViewPost ap = new ViewPost();
                        ap.ImportFromModel(tp);
                        posts.Add(ap);

                    }
                }
                return View(posts);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var mpost = await postManager.Details(id);
                ViewPost post = new ViewPost();
                post.ImportFromModel(mpost);
                
                post.HTMLcontent = MarkUpManager.ConvertToHtml(mpost.content);


                post.Categories = await categoryManager.GetCategoryByPostId((int)id);
                post.CategoriesToString = await categoryManager.GetCategoryNamesToString(post.Blog.Name, (int)id);
                post.Tags = await TagManager.GetTagByPostId((int)id);
                post.TagsToString = await TagManager.GetTagNamesToString(post.Blog.Name, (int)id);



                //QuilDeltaManager quilDeltaManager = new QuilDeltaManager();
                //post.HTMLcontent = quilDeltaManager.ToHrml(mpost.content);
                if (post == null)
                {
                    return NotFound();
                }

                return View(post);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: Posts/Create
        [Authorize]
        public async Task<IActionResult> CreateAsync(string id)
        {
            try
            {
                string blogname = id;


                if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, blogname) == false)
                {
                    return RedirectToAction(nameof(Index), "Posts", new { id = blogname });
                }
                var blog = await this.blmngr.GetBlogAsync(blogname);


                //return View();
                ViewBag.CreateAction = true;


                //string pathbase;
                string pathbase = AppSettingsManager.GetPathBase();
                if (CommonTools.isEmpty(pathbase) == false)
                {
                    ViewBag.pathbase = pathbase;
                }


                if (blog != null)
                {

                    ViewBag.BlogId = blog.Id;
                }
                return View();
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string blogname,[Bind("Id,Title,Published,content,Author,RowVersion,BlogId,engine,CategoriesToString", "TagsToString")] ViewPost post)
        {
            try
            {
                //if (ModelState.IsValid)
                {
                    post.Author = await IDataManager.db.Users.FirstAsync(x => x.UserName == User.Identity.Name);
                    var mpost = post.ToModel(User.Identity.Name);
                    //MarkUpManager MarkUpManager = new MarkUpManager();
                    mpost.content = MarkUpManager.ConvertFromHtmlToMarkUp(post.content);

                    var blog = await blmngr.GetBlogByIdAsync(mpost.BlogId);


                    await postManager.Create((Post)mpost, this.User.Identity.Name);
                    if (post.CategoriesToString != null)
                    {
                        var catgories = post.CategoriesToString.Split(",").ToList();
                        if (catgories != null)
                        {
                            categoryManager.AttachCategoryRangeToPost(catgories, blog.Name, mpost.Id);
                        }
                    }
                    if (post.TagsToString != null)
                    {
                        var tags = post.TagsToString.Split(",").ToList();
                        if (tags != null)
                        {
                            await TagManager.AttachTagRangetoPost(tags, blog.Name, mpost.Id);
                        }
                    }
                    //blmngr.GetBlogAsync()
                    if (!AppSettingsManager.GetEnablePagination())
                    {
                        return RedirectToAction(nameof(Index), "Posts", new { id = blog.Name });
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index), "Posts", new { id = blog.Name,pagge=0 });
                    }
                }
                // return View(post);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id, string blogname)
        {
            try
            {
                if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, blogname) == false)
                {
                    return RedirectToAction(nameof(Index), "Posts", new { id = blogname });
                }

                //return View();

                ViewBag.CreateAction = false;


                //string pathbase;
                string pathbase = AppSettingsManager.GetPathBase();
                if (CommonTools.isEmpty(pathbase) == false)
                {
                    ViewBag.pathbase = pathbase;
                }

                if (id == null)
                {
                    return NotFound();
                }

                var post = await this.postManager.Details(id);
                if (post == null)
                {
                    return NotFound();
                }



                ViewBag.BlogId = post.BlogId;


                var vpost = new ViewPost();
                vpost.ImportFromModel(post);
                //MarkUpManager MarkUpManager = new MarkUpManager();
                //vpost.content = MarkUpManager.ConvertToHtml(post.content);
                //BBCodeManager bBCodeManager = new BBCodeManager();
                //vpost.HTMLcontent = bBCodeManager.ConvertToHtml(post.content);
               // MarkUpManager MarkUpManager = new MarkUpManager();
                vpost.HTMLcontent = MarkUpManager.ConvertToHtml(post.content);
                var cmsengine = AppSettingsManager.GetAppWideCMSEngine();
                if (cmsengine == enumMarkupEngine.QUIL.ToString())
                {
                    vpost.content = vpost.content.Replace("{\"ops\":", "");
                    vpost.content = vpost.content.Remove(vpost.content.Length - 1, 1);
                }
                vpost.CategoriesToString = await categoryManager.GetCategoryNamesToString(vpost.Blog.Name, (int)id);
                vpost.TagsToString = await TagManager.GetTagNamesToString(vpost.Blog.Name, (int)id);
                return View(vpost);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Published,content,Author,RowVersion,BlogId,engine, CategoriesToString", "TagsToString")] ViewPost post, string blogname)
        {
            try
            {
                if (id != post.Id)
                {
                    return NotFound();
                }

                // if (ModelState.IsValid)
              //  {

                    //_context.Update(post);
                    //await _context.SaveChangesAsync();
                    var mpost = post.ToModel(User.Identity.Name);
                //MarkUpManager MarkUpManager = new MarkUpManager();
                mpost.content = MarkUpManager.ConvertFromHtmlToMarkUp(post.content);
               
                mpost = await postManager.Edit(id, (Post)mpost);
                    var blog = await blmngr.GetBlogByIdAsync(mpost.BlogId);
                    if (mpost != null)
                    {
                        post.ImportFromModel(mpost);
                    }
                    if (post.CategoriesToString != null)
                    {
                        var catgories = post.CategoriesToString.Split(",").ToList();
                        if (catgories != null)
                        {
                        
                      
                            categoryManager.DettachCategoryRangetoPost(catgories, blog.Name, mpost.Id);
                        IDataManager.DiconnectAndReconnectToDB();
                         
                        categoryManager.AttachCategoryRangeToPost(catgories, blog.Name, mpost.Id);

                        }
                    }
                if (post.TagsToString != null)
                {
                    var tags = post.TagsToString.Split(",").ToList();
                    if (tags != null)
                    {
                        TagManager.DettachTagRangetoPost(tags, blog.Name, mpost.Id);
                        await TagManager.AttachTagRangetoPost(tags, blog.Name, mpost.Id);

                    }
                }
                // }
                return RedirectToAction(nameof(Index), "Posts", new { id = post.Blog.Name });
            }
                
                catch (DbUpdateConcurrencyException)
                {


                    if ( this.postManager.Exists(post.Id))
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

            //return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id,string blogname)
        {
            try
            {
                if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, blogname) == false)
                {
                    return RedirectToAction(nameof(Index), "Posts", new { id = blogname });
                }
                if (id == null)
                {
                    return NotFound();
                }


                var mpost = await postManager.Details(id);
                ViewPost post = new ViewPost();
                post.ImportFromModel(mpost);
                //MarkUpManager MarkUpManager = new MarkUpManager();
                //post.HTMLcontent = MarkUpManager.ConvertToHtml(mpost.content);
                //BBCodeManager bBCodeManager = new BBCodeManager();
                //post.HTMLcontent = bBCodeManager.ConvertToHtml(mpost.content);
               // MarkUpManager MarkUpManager = new MarkUpManager();
                post.HTMLcontent = MarkUpManager.ConvertToHtml(mpost.content);
                if (post == null)
                {
                    return NotFound();
                }
                ViewBag.BlogName = blogname;
                return View(post);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string blogname)
        {
            try
            {
                //var post = await _context.Post.FindAsync(id);
                //_context.Post.Remove(post);
                //await _context.SaveChangesAsync();
                await this.postManager.Delete(id);


                return RedirectToAction(nameof(Index), "Posts", new { id = blogname });
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
       [HttpPost()]
       
        public async Task<ActionResult> Upload([FromQuery]string bid)
        {
            try
            {
               
              // string Blogid = bid;
                var posts = await postManager.List();
                int postid = -1;
                postid = await  IDataManager.PredictLastId("Post")+1;
                //if(Request.Form.Files.Count==0)
                //{
                //    return null;
                //}

                IFormFile formFile = (FormFile)Request.Form.Files[0];
                
                if(bid==null)
                {
                    bid = Request.RouteValues["id"].ToString();
                }
                var blog = await this.blmngr.GetBlogAsync(bid);
               


                    var path = await fileRecordManager.CreateForBlog(blog.Id, postid, new Files(), formFile,User.Identity.Name);





                // return Content(Url.Content(@"~\Uploads\" + fileid));
                //return Content(path);
                //return Json(new { location = this.HttpContext.Request.Host+"/"+path });
                return Json(new { location = "/" + path });

                // return null;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult> UploadEdit([FromQuery] string bid, [FromQuery] int? pid)
        {
            try
            {
               
                // string Blogid = bid;
               // var posts = await postManager.List();
               // int postid = posts.ToList().Max(x => x.Id) + 1;

                IFormFile formFile = (FormFile)Request.Form.Files[0];
                if (pid == null)
                {
                    pid =(int) Request.RouteValues["id"];//.ToString();
                    
                }
                int bid2 = (await postManager.Details((pid))).BlogId;



                var path = await fileRecordManager.CreateForBlog(bid2, pid, new Files(), formFile, User.Identity.Name);





                // return Content(Url.Content(@"~\Uploads\" + fileid));
                //return Content(path);
                //return Json(new { location = this.HttpContext.Request.Host+"/"+path });
                return Json(new { location = "/" + path });

                // return null;
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
                var posts = await postManager.List();
                int postid = -1;
                postid = await IDataManager.PredictLastId("Post") + 1;
                if (Request.Form.Files.Count == 0)
                {
                    return NoContent();
                }
                string bid=null;
                IFormFile formFile = (FormFile)Request.Form.Files[0];

                if (bid == null)
                {
                    bid = Request.RouteValues["id"].ToString();
                }
                var blog = await this.blmngr.GetBlogAsync(bid);



                var path = await fileRecordManager.CreateForBlog(blog.Id, postid, new Files(), formFile, User.Identity.Name);





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

        //private bool PostExists(int id)
        //{
        //    return _context.Post.Any(e => e.Id == id);
        //}
    }
}
