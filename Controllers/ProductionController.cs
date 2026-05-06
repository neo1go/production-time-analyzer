using Microsoft.AspNetCore.Mvc;

namespace ProductionTimeAnalyzer.Controllers
{
    public class ProductionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
