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
    public class TicketsHistoriesController : Controller
    {
        private readonly FlipFlopContext _context;

        public TicketsHistoriesController(FlipFlopContext context)
        {
            _context = context;
        }

        // GET: TicketsHistories
        public async Task<IActionResult> Index()
        {
            var flipFlopContext = _context.TicketsHistory.Include(t => t.KeyBuyerNavigation).
                                        Include(t => t.KeySellerNavigation).
                                        Include(t => t.KeyTicketNavigation).
                                        Include(t=>t.KeyTicketNavigation.TargetKeyNavigation);
            return View(await flipFlopContext.ToListAsync());
        }

        // GET: TicketsHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketsHistory = await _context.TicketsHistory
                .Include(t => t.KeyBuyerNavigation)
                .Include(t => t.KeySellerNavigation)
                .Include(t => t.KeyTicketNavigation)
                .FirstOrDefaultAsync(m => m.Key == id);
            if (ticketsHistory == null)
            {
                return NotFound();
            }

            return View(ticketsHistory);
        }

        // GET: TicketsHistories/Create
        public IActionResult Create()
        {
            ViewData["KeyBuyer"] = new SelectList(_context.Users, "Key", "City");
            ViewData["KeySeller"] = new SelectList(_context.Users, "Key", "City");
            ViewData["KeyTicket"] = new SelectList(_context.PlainTickets, "Key", "FlightNumber");
            return View();
        }

        // POST: TicketsHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Key,KeySeller,KeyBuyer,KeyTicket,DateOfTrade")] TicketsHistory ticketsHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketsHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KeyBuyer"] = new SelectList(_context.Users, "Key", "City", ticketsHistory.KeyBuyer);
            ViewData["KeySeller"] = new SelectList(_context.Users, "Key", "City", ticketsHistory.KeySeller);
            ViewData["KeyTicket"] = new SelectList(_context.PlainTickets, "Key", "FlightNumber", ticketsHistory.KeyTicket);
            return View(ticketsHistory);
        }

        // GET: TicketsHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketsHistory = await _context.TicketsHistory.FindAsync(id);
            if (ticketsHistory == null)
            {
                return NotFound();
            }
            ViewData["KeyBuyer"] = new SelectList(_context.Users, "Key", "City", ticketsHistory.KeyBuyer);
            ViewData["KeySeller"] = new SelectList(_context.Users, "Key", "City", ticketsHistory.KeySeller);
            ViewData["KeyTicket"] = new SelectList(_context.PlainTickets, "Key", "FlightNumber", ticketsHistory.KeyTicket);
            return View(ticketsHistory);
        }

        // POST: TicketsHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Key,KeySeller,KeyBuyer,KeyTicket,DateOfTrade")] TicketsHistory ticketsHistory)
        {
            if (id != ticketsHistory.Key)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketsHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketsHistoryExists(ticketsHistory.Key))
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
            ViewData["KeyBuyer"] = new SelectList(_context.Users, "Key", "City", ticketsHistory.KeyBuyer);
            ViewData["KeySeller"] = new SelectList(_context.Users, "Key", "City", ticketsHistory.KeySeller);
            ViewData["KeyTicket"] = new SelectList(_context.PlainTickets, "Key", "FlightNumber", ticketsHistory.KeyTicket);
            return View(ticketsHistory);
        }

        // GET: TicketsHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketsHistory = await _context.TicketsHistory
                .Include(t => t.KeyBuyerNavigation)
                .Include(t => t.KeySellerNavigation)
                .Include(t => t.KeyTicketNavigation)
                .FirstOrDefaultAsync(m => m.Key == id);
            if (ticketsHistory == null)
            {
                return NotFound();
            }

            return View(ticketsHistory);
        }

        // POST: TicketsHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketsHistory = await _context.TicketsHistory.FindAsync(id);
            _context.TicketsHistory.Remove(ticketsHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketsHistoryExists(int id)
        {
            return _context.TicketsHistory.Any(e => e.Key == id);
        }
    }
}
