﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using flip_flop.Models;

namespace flip_flop.Controllers
{
    public class ComplainsStatusController : Controller
    {
        private readonly FlipFlopContext _context;

        public ComplainsStatusController(FlipFlopContext context)
        {
            _context = context;
        }

        // GET: ComplainsStatus
        public async Task<IActionResult> Index()
        {
            var flipFlopContext = _context.ComplainsStatus.Include(c => c.ComplainKeyNavigation);
            return View(await flipFlopContext.ToListAsync());
        }

        // GET: ComplainsStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complainsStatus = await _context.ComplainsStatus
                .Include(c => c.ComplainKeyNavigation)
                .FirstOrDefaultAsync(m => m.Key == id);
            if (complainsStatus == null)
            {
                return NotFound();
            }

            return View(complainsStatus);
        }

        // GET: ComplainsStatus/Create
        public IActionResult Create()
        {
            ViewData["ComplainKey"] = new SelectList(_context.Complains, "Key", "Content");
            return View();
        }

        // POST: ComplainsStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Key,ComplainKey,Status,Comments")] ComplainsStatus complainsStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(complainsStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ComplainKey"] = new SelectList(_context.Complains, "Key", "Content", complainsStatus.ComplainKey);
            return View(complainsStatus);
        }

        // GET: ComplainsStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complainsStatus = await _context.ComplainsStatus.FindAsync(id);
            if (complainsStatus == null)
            {
                return NotFound();
            }
            ViewData["ComplainKey"] = new SelectList(_context.Complains, "Key", "Content", complainsStatus.ComplainKey);
            return View(complainsStatus);
        }

        // POST: ComplainsStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Key,ComplainKey,Status,Comments")] ComplainsStatus complainsStatus)
        {
            if (id != complainsStatus.Key)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(complainsStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComplainsStatusExists(complainsStatus.Key))
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
            ViewData["ComplainKey"] = new SelectList(_context.Complains, "Key", "Content", complainsStatus.ComplainKey);
            return View(complainsStatus);
        }

        // GET: ComplainsStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complainsStatus = await _context.ComplainsStatus
                .Include(c => c.ComplainKeyNavigation)
                .FirstOrDefaultAsync(m => m.Key == id);
            if (complainsStatus == null)
            {
                return NotFound();
            }

            return View(complainsStatus);
        }

        // POST: ComplainsStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var complainsStatus = await _context.ComplainsStatus.FindAsync(id);
            _context.ComplainsStatus.Remove(complainsStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComplainsStatusExists(int id)
        {
            return _context.ComplainsStatus.Any(e => e.Key == id);
        }
    }
}