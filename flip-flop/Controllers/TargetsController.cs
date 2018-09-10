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
    public class TargetsController : Controller
    {
        private readonly FlipFlopContext _context;

        public TargetsController(FlipFlopContext context)
        {
            _context = context;
        }

        // GET: Targets
        public async Task<IActionResult> Index()
        {
            var flipFlopContext = _context.Targets.Include(t => t.CountryKeyNavigation);
            return View(await flipFlopContext.ToListAsync());
        }

        // GET: Targets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targets = await _context.Targets
                .Include(t => t.CountryKeyNavigation)
                .FirstOrDefaultAsync(m => m.Key == id);
            if (targets == null)
            {
                return NotFound();
            }

            return View(targets);
        }

        // GET: Targets/Create
        public IActionResult Create()
        {
            ViewData["CountryKey"] = new SelectList(_context.Countries, "Key", "CountryName");
            return View();
        }

        // POST: Targets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Key,CountryKey,CityName")] Targets targets)
        {
            if (ModelState.IsValid)
            {
                _context.Add(targets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryKey"] = new SelectList(_context.Countries, "Key", "CountryName", targets.CountryKey);
            return View(targets);
        }

        // GET: Targets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targets = await _context.Targets.FindAsync(id);
            if (targets == null)
            {
                return NotFound();
            }
            ViewData["CountryKey"] = new SelectList(_context.Countries, "Key", "CountryName", targets.CountryKey);
            return View(targets);
        }

        // POST: Targets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Key,CountryKey,CityName")] Targets targets)
        {
            if (id != targets.Key)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(targets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TargetsExists(targets.Key))
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
            ViewData["CountryKey"] = new SelectList(_context.Countries, "Key", "CountryName", targets.CountryKey);
            return View(targets);
        }

        // GET: Targets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var targets = await _context.Targets
                .Include(t => t.CountryKeyNavigation)
                .FirstOrDefaultAsync(m => m.Key == id);
            if (targets == null)
            {
                return NotFound();
            }

            return View(targets);
        }

        // POST: Targets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var targets = await _context.Targets.FindAsync(id);
            _context.Targets.Remove(targets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TargetsExists(int id)
        {
            return _context.Targets.Any(e => e.Key == id);
        }
    }
}
