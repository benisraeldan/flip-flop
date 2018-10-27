using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using flip_flop.Models;
using flip_flop.WebService;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace flip_flop.Controllers
{
    public class HomeController : Controller
    {
        private readonly FlipFlopContext _context;


        public HomeController(FlipFlopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            ViewBag.countries = await _context.Countries.Select(t => t.CountryName).ToListAsync();
           
            WeatherData nyc = new WeatherData("New york");
            WeatherData paris = new WeatherData("Paris");
            WeatherData london = new WeatherData("London");
            nyc.CheckWeather();
            paris.CheckWeather();
            london.CheckWeather();

            ViewData["New-York"] = nyc.ToString();
            ViewData["Paris"] = paris.ToString();
            ViewData["London"] = london.ToString();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
