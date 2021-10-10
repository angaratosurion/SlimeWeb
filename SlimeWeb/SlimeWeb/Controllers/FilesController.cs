using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;
using SlimeWeb.Core.Data.ViewModels;
using SlimeWeb.Core.Managers;

namespace SlimeWeb.Controllers
{
    public class FilesController : Controller
    {
        private readonly SlimeDbContext _context;
        private readonly FileRecordManager fileRecordManager;
        AccessManager accessManager = new AccessManager();
        BlogManager blogManager = new BlogManager();
        public FilesController(SlimeDbContext context)
        {
            _context = context;
            fileRecordManager = new FileRecordManager();
        }

        // GET: Files
        public async Task<IActionResult> Index(string id)
        {

            try
            {
                List<ViewFiles> lstFiles = new List<ViewFiles>();

                string name = id;


                if (name == null)
                {
                    return NotFound();
                }
                var files = await this.fileRecordManager.GetFilesByBlogName(name);
                foreach(var f in files)
                {
                    ViewFiles vf = new ViewFiles();
                    vf.ImportFromModel(f);
                    lstFiles.Add(vf);

                }
                return View(lstFiles);

            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var files = await this.fileRecordManager.Details((int)id);
            if (files == null)
            {
                return NotFound();
            }
            ViewFiles ap = new ViewFiles();
          
            ap.ImportFromModel(files);
           

            return View(ap);
        }
        //[Authorize]
        //// GET: Files/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Files/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,FileName,Path,RelativePath,RowVersion,ContentType,Owner")] Files files)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(files);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(files);
        //}
        [Authorize]
        // GET: Files/Edit/5
        public async Task<IActionResult> Edit(int? id, string blogname)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (await this.accessManager.DoesUserHasAccess(User.Identity.Name, blogname) == false)
            {
                return RedirectToAction(nameof(Details), "Files", new { id = id});
            }

            var files = await this.fileRecordManager.Details((int)id);
            ViewFiles ap = new ViewFiles();
            if (files == null)
            {
                return NotFound();
            }
            ap.ImportFromModel(files);           
            return View(ap);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FileName,Path,RelativePath,RowVersion,ContentType,Owner")] Files files)
        {
            if (id != files.Id)
            {
                return NotFound();
            }

            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(files);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilesExists(files.Id))
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
            return View(files);
        }
        [Authorize]
        // GET: Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var files = await this.fileRecordManager.Details((int)id);
            if (files == null)
            {
                return NotFound();
            }

            var blog = await this.fileRecordManager.GetBlofByFileId((int)id);
            if (blog==null)
            {
                return NotFound();
            }
            if (await this.accessManager.DoesUserHasAccess(User.Identity.Name,blog.Name) == false)
            {
                return RedirectToAction(nameof(Details), "Files", new { id =id });
            }

            return View(files);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var files = await _context.Files.FindAsync(id);
            _context.Files.Remove(files);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilesExists(int id)
        {
            return _context.Files.Any(e => e.Id == id);
        }
    }
}
