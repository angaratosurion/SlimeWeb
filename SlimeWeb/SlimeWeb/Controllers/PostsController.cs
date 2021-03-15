using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SlimeWeb.Core;
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
        CategoryManager CategoryManager = new CategoryManager();
      
         
        public PostsController(SlimeDbContext context)
        {
            _context = context;
            postManager = new PostManager( );
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
          
            var mpost = await postManager.Details(id);
            ViewPost post = new ViewPost();
            post.ImportFromModel(mpost);
            MarkDownManager markDownManager = new MarkDownManager();
            post.HTMLcontent = markDownManager.ConvertToHtml(mpost.content);
            post.Categories = await CategoryManager.GetCategoryByPostId((int)id);



            //QuilDeltaManager quilDeltaManager = new QuilDeltaManager();
            //post.HTMLcontent = quilDeltaManager.ToHrml(mpost.content);
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
            string pathwithextention = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string path = System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\", "");
            //return View();
            var builder = new ConfigurationBuilder()
                            .SetBasePath(path)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var config = builder.Build();//string pathbase;
            string pathbase = config.GetValue<string>("ApppSettings:PathBase");
            if ( CommonTools.isEmpty(pathbase)==false)
            {
                ViewBag.pathbase = pathbase;
            }


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
        public async Task<IActionResult> Create(string blogname,[Bind("Id,Title,Published,content,Author,RowVersion,BlogId,engine,CategoriesToString")] ViewPost post)
        {
            //if (ModelState.IsValid)
            {
                var mpost = post.ToModel();
                MarkDownManager markDownManager = new MarkDownManager();
                mpost.content = markDownManager.ConvertFromHtmlToMarkDwon(post.content);
               var blog=await blmngr.GetBlogByIdAsync(mpost.BlogId);
              
                 
                await postManager.Create(mpost, this.User.Identity.Name);
                if( post.CategoriesToString!=null)
                {
                    var catgories = post.CategoriesToString.Split(",").ToList();
                    if(catgories!=null)
                    {
                        CategoryManager.AttachCategoryRangetoPost(catgories,blog.Name, mpost.Id);
                    }
                }
                //blmngr.GetBlogAsync()
                return RedirectToAction(nameof(Index),"Posts",new { id = blog.Name });
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

            var post = await this.postManager.Details(id);
            if (post == null)
            {
                return NotFound();
            }


            ViewBag.BlogId = post.BlogId;
            
            var vpost = new ViewPost();
            vpost.ImportFromModel(post);
            MarkDownManager markDownManager = new MarkDownManager();
            vpost.content = markDownManager.ConvertToHtml(post.content);
            return View(vpost);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Published,content,Author,RowVersion,BlogId,engine")] ViewPost post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(post);
                    //await _context.SaveChangesAsync();
                    var mpost = post.ToModel();
                    MarkDownManager markDownManager = new MarkDownManager();
                    mpost.content = markDownManager.ConvertFromHtmlToMarkDwon(post.content);
                    mpost=await postManager.Edit(id,mpost);
                    if (mpost  != null)
                    {
                        post.ImportFromModel(mpost);
                    }
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
                return RedirectToAction(nameof(Index), "Posts", new { id = post.Blog.Name });
            }
            //return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mpost = await postManager.Details(id);
            ViewPost post = new ViewPost();
            post.ImportFromModel(mpost);
            MarkDownManager markDownManager = new MarkDownManager();
            post.HTMLcontent = markDownManager.ConvertToHtml(mpost.content);
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
            //var post = await _context.Post.FindAsync(id);
            //_context.Post.Remove(post);
            //await _context.SaveChangesAsync();
           await  this.postManager.Delete(id);
            return RedirectToAction(nameof(Index));
        }
      //  [HttpPost]
       
        public async Task<ActionResult> Upload([FromQuery]string bid)
        {
            try
            {
                FileRecordManager fileRecordManager = new FileRecordManager();
              // string Blogid = bid;
                var posts = await postManager.List();
                int postid = -1;
                postid = await  fileRecordManager.PredictLastId("Post")+1;
                

                IFormFile formFile = (FormFile)Request.Form.Files[0];
                if(bid==null)
                {
                    bid = Request.RouteValues["id"].ToString();
                }
                var blog = await this.blmngr.GetBlogAsync(bid);
               


                    var path = await fileRecordManager.Create(blog.Id, postid, new Files(), formFile,User.Identity.Name);





                // return Content(Url.Content(@"~\Uploads\" + fileid));
                //return Content(path);
                //return Json(new { location = this.HttpContext.Request.Host+"/"+path });
                return Json(new { location = "/" + path });

                // return null;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<ActionResult> UploadEdit([FromQuery] string bid, [FromQuery] int? pid)
        {
            try
            {
                FileRecordManager fileRecordManager = new FileRecordManager();
                // string Blogid = bid;
               // var posts = await postManager.List();
               // int postid = posts.ToList().Max(x => x.Id) + 1;

                IFormFile formFile = (FormFile)Request.Form.Files[0];
                if (pid == null)
                {
                    pid =(int) Request.RouteValues["id"];//.ToString();
                    
                }
                int bid2 = (await postManager.Details((pid))).BlogId;



                var path = await fileRecordManager.Create(bid2, pid, new Files(), formFile, User.Identity.Name);





                // return Content(Url.Content(@"~\Uploads\" + fileid));
                //return Content(path);
                //return Json(new { location = this.HttpContext.Request.Host+"/"+path });
                return Json(new { location = "/" + path });

                // return null;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        //private bool PostExists(int id)
        //{
        //    return _context.Post.Any(e => e.Id == id);
        //}
    }
}
