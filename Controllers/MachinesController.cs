using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionTimeAnalyzer.Data;

namespace ProductionTimeAnalyzer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachinesController : ControllerBase
    {
        private readonly ProductionTimeAnalyzerContext _context;

        public MachinesController(ProductionTimeAnalyzerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var machines = await _context.Machines
                .Select(m => new
                {
                    m.Id,
                    m.Name
                })
                .ToListAsync();

            return Ok(machines);
        }
    }
}