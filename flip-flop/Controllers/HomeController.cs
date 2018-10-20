using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using flip_flop.Models;
using flip_flop.WebService;

namespace flip_flop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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
