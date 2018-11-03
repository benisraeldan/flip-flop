using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using flip_flop.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using libsvm;
using SVMTextClassifier;
using System.Diagnostics;

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
            var context = _context.PlainTickets.Include(t => t.ClassKeyNavigation).
                                        Include(t => t.Owner).
                                        Include(t => t.TargetKeyNavigation).
                                        Include(t=> t.TargetKeyNavigation.CountryKeyNavigation);

            return View(await context.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string searchString , string Destination, string UserName)
        {
            var PlainTickets = from m in _context.PlainTickets
                         select m;

            //TODO: remove my flights
            //PlainTickets = PlainTickets.Where(x=> x.OwnerId ==)
            //TODO: if user is not admin need to show only not sold
            //PlainTickets = PlainTickets.Where(s => s.IsSold);
            if (!String.IsNullOrEmpty(searchString))
            {
                PlainTickets = PlainTickets.Where(s => s.FlightNumber.ToLower().Contains(searchString));
            }
            if (!String.IsNullOrEmpty(Destination))
            {
                PlainTickets = PlainTickets.Where(s => s.TargetKeyNavigation.CityName.ToLower().Contains(Destination));
            }
            if (!String.IsNullOrEmpty(UserName))
            {
                //PlainTickets = PlainTickets.Where(s => s.Owner.FirstName.ToLower().Contains(UserName));
            }

            PlainTickets = PlainTickets.Include(t => t.ClassKeyNavigation).
                                        Include(t => t.Owner).
                                        Include(t => t.TargetKeyNavigation).
                                        Include(t => t.TargetKeyNavigation.CountryKeyNavigation);

            //PlainTickets = PlainTickets.Where(s => s.IsSold == false);

            return View(await PlainTickets.ToListAsync());
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

        public JsonResult ReturnJsonSubCategories(int categoryId)
        {
            var jsonData = _context.Targets.Where(x => x.Key == categoryId).ToList();
            return Json(jsonData);
        }

        // GET: PlainTickets/Create
        public IActionResult Create()
        {
            // Create the select list for departments field
            if (ViewBag.ListOfDepartments == null)
            {
                var DepartmentList = (from product in _context.Department select product);

                DepartmentList.OrderBy(x => x.Type);

                ViewBag.ListOfDepartments = new SelectList(DepartmentList.AsNoTracking(), "Key", "Type");
            }

            // Create the select list for target field
            if (ViewBag.ListOfTargets == null)
            {
                var targetsList = (from product in _context.Targets select product);

                targetsList.OrderBy(x => x.CityName);
                
                ViewBag.ListOfTargets = new SelectList(targetsList.AsNoTracking(), "Key", "CityName");
            }

            ViewData["CountryKey"] = new SelectList(_context.Countries.AsNoTracking(), "Key", "CountryName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyFlight([Bind("Key,OwnerId")] PlainTickets plainTickets)
        {
            if (ModelState.IsValid)
            {
                // Add to history
                // TODO: replace t with current user
                TicketsHistory ticketsHistory = new TicketsHistory(5, plainTickets.OwnerId, plainTickets.Key);
                _context.Add(ticketsHistory);

                // Remove from plain tickets
                var a = _context.PlainTickets.First(x => x.Key == plainTickets.Key);
                a.IsSold = true;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            
            return View(plainTickets);
        }

        // POST: PlainTickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Key,Target,DateOfFlight,FlightNumber,CancleFee,Food,Baggage,Class,Price")] PlainTickets plainTickets)
        {
            if (ModelState.IsValid)
            {
                plainTickets.IsSold = false;
                //TODO: add owner id
                //plainTickets.OwnerId =                 
                _context.Add(plainTickets);
                await _context.SaveChangesAsync();

                try
                {
                    var accessToken = "EAAehTCoOZARcBAMI9fOZBrkrAEVk1YZCsdnGUBL7ZB3fs9sRpN6nIhC6zHxhXfFmnUQj0EM4BBbQuijVyXqdRfHWP3zmw3ZCykKsTMYv2CCcptDbEV46MZC6JB9cDxNZCbewoEe68FecNTQ1aQz0biRNzPq9f1CPFAyQAIywLZAObeDXs8ZBQy2dZBG4dFwmcoE0MZD";
                    var facebookClient = new FacebookClient();
                    var facebookService = new FacebookService(facebookClient);
                   
                    var postOnWallTask = facebookService.PostOnWallAsync(accessToken, "New destination" + plainTickets.Price);
                    Task.WaitAll(postOnWallTask);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(plainTickets);
        }



        private static IEnumerable<string> GetWords(string x)
        {
            return x.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTarget([Bind("Key,CountryName,CityName")] Targets targets)
        {
            if (ModelState.IsValid)
            {
                _context.Add(targets);
                await _context.SaveChangesAsync();
                return this.RedirectToAction(nameof(Create));
            }
            ViewData["CountryKey"] = new SelectList(_context.Countries, "Key", "CountryName", targets.CountryName);
            return View(targets);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCountry([Bind("Key", "CountryName")] Countries countries)
        {
            if (ModelState.IsValid)
            {
                _context.Add(countries);
                await _context.SaveChangesAsync();
            }
               ViewData["CountryKey"] = new SelectList(_context.Countries, "Key", "CountryName");
            return this.RedirectToAction(nameof(Create));
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


public interface IFacebookService
{
    Task PostOnWallAsync(string accessToken, string message);
}


public interface IFacebookClient
{
    Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null);
    Task PostAsync(string accessToken, string endpoint, object data, string args = null);
}

public class FacebookService : IFacebookService
{
    private readonly IFacebookClient _facebookClient;

    public FacebookService(IFacebookClient facebookClient)
    {
        _facebookClient = facebookClient;
    }
    
    public async Task PostOnWallAsync(string accessToken, string message)
        => await _facebookClient.PostAsync(accessToken, "me/feed", new { message });

}





public class FacebookClient : IFacebookClient
{
    private readonly HttpClient _httpClient;

    public FacebookClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://graph.facebook.com/v3.1/")
        };
        _httpClient.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null)
    {
        var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
        if (!response.IsSuccessStatusCode)
            return default(T);

        var result = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(result);
    }

    public async Task PostAsync(string accessToken, string endpoint, object data, string args = null)
    {
        var payload = GetPayload(data);
        var response = await _httpClient.PostAsync($"{endpoint}?access_token={accessToken}&{args}", payload);
    }

    private static StringContent GetPayload(object data)
    {
        var json = JsonConvert.SerializeObject(data);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
