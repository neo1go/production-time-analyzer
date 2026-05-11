using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProductionTimeAnalyzer.Controllers
{
    [Authorize]
    public class ProductionController : Controller
    {
        public IActionResult Overview()
        {
            return View();
        }
    }
}
