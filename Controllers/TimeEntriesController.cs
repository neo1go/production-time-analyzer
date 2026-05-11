using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionTimeAnalyzer.Data;
using ProductionTimeAnalyzer.Dtos;


namespace ProductionTimeAnalyzer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ProductionTimeAnalyzerContext _context;

        //Konstruktor
        public TimeEntriesController(ProductionTimeAnalyzerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
         DateTime? startDate,
         DateTime? endDate,
         int? machineId)
        {
            var query = _context.TimeEntries
                .Include(t => t.Product)
                .Include(t => t.Machine)
                .AsQueryable();  // EF Core macht daraus einen SQL-Befehl

            if (startDate.HasValue)
            {
                query = query.Where(t => t.StartTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                var inclusiveEnd = endDate.Value.Date.AddDays(1); // so wird der ganze Tag inkludiert
                query = query.Where(t => t.EndTime < inclusiveEnd);
            }

            if (machineId.HasValue)
            {
                query = query.Where(t => t.MachineId == machineId.Value);
            }

            var result = await query
                .Select(t => new TimeEntryDto
                {
                    Id = t.Id,
                    ProductName = t.Product.Name,
                    MachineName = t.Machine.Name,
                    Status = t.Status.ToString(),
                    StartTime = t.StartTime,
                    EndTime = t.EndTime
                })
                .ToListAsync();

            return Ok(result);
        }


    }
}
