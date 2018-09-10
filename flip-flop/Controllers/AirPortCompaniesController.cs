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
    public class AirPortCompaniesController : Controller
    {
        private readonly FlipFlopContext _context;

        public AirPortCompaniesController(FlipFlopContext context)
        {
            _context = context;
        }

        // GET: AirPortCompanies
        public async Task<IActionResult> Index()
        {
            return View(await _context.AirPortCompany.ToListAsync());
        }

        // GET: AirPortCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPortCompany = await _context.AirPortCompany
                .FirstOrDefaultAsync(m => m.Key == id);
            if (airPortCompany == null)
            {
                return NotFound();
            }

            return View(airPortCompany);
        }

        // GET: AirPortCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AirPortCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Key,Name")] AirPortCompany airPortCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airPortCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airPortCompany);
        }

        // GET: AirPortCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPortCompany = await _context.AirPortCompany.FindAsync(id);
            if (airPortCompany == null)
            {
                return NotFound();
            }
            return View(airPortCompany);
        }

        // POST: AirPortCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Key,Name")] AirPortCompany airPortCompany)
        {
            if (id != airPortCompany.Key)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airPortCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirPortCompanyExists(airPortCompany.Key))
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
            return View(airPortCompany);
        }

        // GET: AirPortCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airPortCompany = await _context.AirPortCompany
                .FirstOrDefaultAsync(m => m.Key == id);
            if (airPortCompany == null)
            {
                return NotFound();
            }

            return View(airPortCompany);
        }

        // POST: AirPortCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airPortCompany = await _context.AirPortCompany.FindAsync(id);
            _context.AirPortCompany.Remove(airPortCompany);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirPortCompanyExists(int id)
        {
            return _context.AirPortCompany.Any(e => e.Key == id);
        }
    }
}
