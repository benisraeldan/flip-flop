using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using flip_flop.Models;
using SVMTextClassifier;
using libsvm;

namespace flip_flop.Controllers
{
    public class TicketsHistoriesController : Controller
    {
        private readonly FlipFlopContext _context;
        private List<string> vocabulary = new List<string>();

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
                                        Include(t => t.KeyTicketNavigation.TargetKeyNavigation);
            ViewBag.users = flipFlopContext.GroupBy(x => x.KeyBuyerNavigation).ToList();


         

            return View(await flipFlopContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Owner, string Buyer, DateTime DateOfFlight)
        {
            var History = from m in _context.TicketsHistory
                          select m;

            if (!String.IsNullOrEmpty(Owner))
            {
                History = History.Where(s => s.KeySellerNavigation.FirstName.ToLower().Contains(Owner));
            }
            if (!String.IsNullOrEmpty(Buyer))
            {
                History = History.Where(s => s.KeyBuyerNavigation.FirstName.ToLower().Contains(Buyer));
            }
            if (DateOfFlight.ToShortDateString() != "01/01/0001")
            {

                History = History.Where(s => s.DateOfTrade == DateOfFlight);
            }

            History = History.Include(t => t.KeyBuyerNavigation).
                                        Include(t => t.KeySellerNavigation).
                                        Include(t => t.KeyTicketNavigation).
                                        Include(t => t.KeyTicketNavigation.TargetKeyNavigation);
            //PlainTickets = PlainTickets.Where(s => s.IsSold == false);

            return View(await History.ToListAsync());
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
        public async Task<IActionResult> Edit()
        {
            var helper = new Dictionary<string, string>();
            var model = getmodel();
            foreach (var countries in _context.Countries)
            {
                var a = svn(model, countries.CountryName);
                helper.Add(countries.CountryName, a);
            }

            ViewBag.svn = helper;

            var ticketsHistory = await _context.TicketsHistory.FindAsync(1);
            if (ticketsHistory == null)
            {
                return NotFound();
            }
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

        public C_SVC getmodel()
        {
            List<string> x = new List<string>();
            List<double> yb = new List<double>();

            foreach (var obj in _context.PlainTickets)
            {
                double val = -1;

                x.Add(_context.Countries.Where(ct => ct.Key == _context.Targets.
                                         Where(t => t.Key == obj.Target).FirstOrDefault().CountryName).
                                         FirstOrDefault().CountryName);

                if (obj.IsSold)
                {
                    val = 1;
                }

                yb.Add(val);
            }

            double[] y = yb.ToArray();
            this.vocabulary = x.SelectMany(GetWords).Distinct().OrderBy(word => word).ToList();

            var problemBuilder = new TextClassificationProblemBuilder();

            var problem = problemBuilder.CreateProblem(x, y, vocabulary.ToList());

            const int C = 1;

            var model = new C_SVC(problem, KernelHelper.LinearKernel(), C);

            return (model);
        }

        public static IEnumerable<string> GetWords(string x)
        {
            return x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string svn(C_SVC model, string country_name)
        {

            var accuracy = model.GetCrossValidationAccuracy(10);

            var _predictionDictionary = new Dictionary<int, string> { { -1, "no" }, { 1, "yes" } };



            var newX = TextClassificationProblemBuilder.CreateNode(country_name, vocabulary);

            var predictedY = model.Predict(newX);

            return (_predictionDictionary[(int)predictedY]);
            //Debug.WriteLine("The prediction is " + _predictionDictionary[(int)predictedY]);


        }
    }
}