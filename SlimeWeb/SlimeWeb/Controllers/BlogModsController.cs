using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SlimeWeb.Core.Data.DBContexts;
using SlimeWeb.Core.Data.Models;

namespace SlimeWeb.Controllers
{
    public class BlogModsController : Controller
    {
        private readonly SlimeDbContext _context;

        public BlogModsController(SlimeDbContext context)
        {
            _context = context;
        }

        // GET: BlogMods
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogMods.ToListAsync());
        }

        // GET: BlogMods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogMods = await _context.BlogMods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogMods == null)
            {
                return NotFound();
            }

            return View(blogMods);
        }

        // GET: BlogMods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogMods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Moderator")] BlogMods blogMods)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogMods);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogMods);
        }

        // GET: BlogMods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogMods = await _context.BlogMods.FindAsync(id);
            if (blogMods == null)
            {
                return NotFound();
            }
            return View(blogMods);
        }

        // POST: BlogMods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Moderator")] BlogMods blogMods)
        {
            if (id != blogMods.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogMods);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogModsExists(blogMods.Id))
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
            return View(blogMods);
        }

        // GET: BlogMods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogMods = await _context.BlogMods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogMods == null)
            {
                return NotFound();
            }

            return View(blogMods);
        }

        // POST: BlogMods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogMods = await _context.BlogMods.FindAsync(id);
            _context.BlogMods.Remove(blogMods);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogModsExists(int id)
        {
            return _context.BlogMods.Any(e => e.Id == id);
        }
    }
}
