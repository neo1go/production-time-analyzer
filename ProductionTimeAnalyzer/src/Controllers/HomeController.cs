using Microsoft.AspNetCore.Mvc;
using ProductionTimeAnalyzer.Models;
using System.Diagnostics;

namespace ProductionTimeAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return View(); //die ASP.net Autostartseite wird somit umgangen
            return RedirectToAction("Overview","Production");
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
