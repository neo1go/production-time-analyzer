using Microsoft.AspNetCore.Mvc;

namespace ProductionTimeAnalyzer.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
