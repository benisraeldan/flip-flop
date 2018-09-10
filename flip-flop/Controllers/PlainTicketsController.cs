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
    public class PlainTicketsController : Controller
    {
        private readonly FlipFlopContext _context;

        public PlainTicketsController(FlipFlopContext context)
        {
            _context = context;
        }

        // GET: PlainTickets
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlainTickets.ToListAsync());
        }

        // GET: PlainTickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plainTickets = await _context.PlainTickets
                .FirstOrDefaultAsync(m => m.Key == id);
            if (plainTickets == null)
            {
                return NotFound();
            }

            return View(plainTickets);
        }

        // GET: PlainTickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlainTickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Key,TargetKey,DateOfFlight,FlightNumber,OwnerId,CancleFee,Food,Baggage,ClassKey,Price")] PlainTickets plainTickets)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plainTickets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plainTickets);
        }

        // GET: PlainTickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plainTickets = await _context.PlainTickets.FindAsync(id);
            if (plainTickets == null)
            {
                return NotFound();
            }
            return View(plainTickets);
        }

        // POST: PlainTickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Key,TargetKey,DateOfFlight,FlightNumber,OwnerId,CancleFee,Food,Baggage,ClassKey,Price")] PlainTickets plainTickets)
        {
            if (id != plainTickets.Key)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plainTickets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlainTicketsExists(plainTickets.Key))
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
            return View(plainTickets);
        }

        // GET: PlainTickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plainTickets = await _context.PlainTickets
                .FirstOrDefaultAsync(m => m.Key == id);
            if (plainTickets == null)
            {
                return NotFound();
            }

            return View(plainTickets);
        }

        // POST: PlainTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plainTickets = await _context.PlainTickets.FindAsync(id);
            _context.PlainTickets.Remove(plainTickets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlainTicketsExists(int id)
        {
            return _context.PlainTickets.Any(e => e.Key == id);
        }
    }
}
