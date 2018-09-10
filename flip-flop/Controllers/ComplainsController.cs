using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using flip_flop.Models;

namespace flip_flop.Controllers
{
    public class ComplainsController : Controller
    {
        private readonly FlipFlopContext _context;

        public ComplainsController(FlipFlopContext context)
        {
            _context = context;
        }

        // GET: Complains
        public async Task<IActionResult> Index()
        {
            var flipFlopContext = _context.Complains.Include(c => c.UserKeyNavigation);
            return View(await flipFlopContext.ToListAsync());
        }

        // GET: Complains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complains = await _context.Complains
                .Include(c => c.UserKeyNavigation)
                .FirstOrDefaultAsync(m => m.Key == id);
            if (complains == null)
            {
                return NotFound();
            }

            return View(complains);
        }

        // GET: Complains/Create
        public IActionResult Create()
        {
            ViewData["UserKey"] = new SelectList(_context.Users, "Key", "City");
            return View();
        }

        // POST: Complains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Key,Title,UserKey,Content,Date")] Complains complains)
        {
            if (ModelState.IsValid)
            {
                _context.Add(complains);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserKey"] = new SelectList(_context.Users, "Key", "City", complains.UserKey);
            return View(complains);
        }

        // GET: Complains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complains = await _context.Complains.FindAsync(id);
            if (complains == null)
            {
                return NotFound();
            }
            ViewData["UserKey"] = new SelectList(_context.Users, "Key", "City", complains.UserKey);
            return View(complains);
        }

        // POST: Complains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Key,Title,UserKey,Content,Date")] Complains complains)
        {
            if (id != complains.Key)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(complains);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComplainsExists(complains.Key))
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
            ViewData["UserKey"] = new SelectList(_context.Users, "Key", "City", complains.UserKey);
            return View(complains);
        }

        // GET: Complains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complains = await _context.Complains
                .Include(c => c.UserKeyNavigation)
                .FirstOrDefaultAsync(m => m.Key == id);
            if (complains == null)
            {
                return NotFound();
            }

            return View(complains);
        }

        // POST: Complains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var complains = await _context.Complains.FindAsync(id);
            _context.Complains.Remove(complains);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComplainsExists(int id)
        {
            return _context.Complains.Any(e => e.Key == id);
        }
    }
}
