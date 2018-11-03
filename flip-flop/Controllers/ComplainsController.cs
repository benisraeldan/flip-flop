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
                complains.Date = DateTime.Now;
                complains.UserKey = 5;
                _context.Add(complains);
                var complainStatus = new ComplainsStatus(complains.Key);
                _context.Add(complainStatus);

                await _context.SaveChangesAsync();
                return Redirect("/Home/Index");
            }
            ViewData["UserKey"] = new SelectList(_context.Users, "Key", "City", complains.UserKey);
            return View(complains);
        }


        private bool ComplainsExists(int id)
        {
            return _context.Complains.Any(e => e.Key == id);
        }
    }
}
