using Microsoft.AspNetCore.Mvc;
using ProductionTimeAnalyzer.Data;
using ProductionTimeAnalyzer.Dtos;
using ProductionTimeAnalyzer.Services;
using Microsoft.EntityFrameworkCore;

namespace ProductionTimeAnalyzer.Controllers
{

    [ApiController]
    [Route("api/analysis")]
    public class AnalysisController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TimeAnalysisService _analysisService;

        public AnalysisController(
            AppDbContext context,
            TimeAnalysisService analysisService)
        {
            _context = context;
            _analysisService = analysisService;
        }

        [HttpGet]
        public async Task<ActionResult<TimeAnalysisDto>> GetAnalysis(
            DateTime? startDate,
            DateTime? endDate,
            int? machineId)
        {
            var query = _context.TimeEntries.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(t => t.StartTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(t => t.EndTime <= endDate.Value);

            if (machineId.HasValue)
                query = query.Where(t => t.MachineId == machineId.Value);

            var entries = await query.ToListAsync();

            // ✅ HIER der Aufruf
            var analysis = _analysisService.Analyze(entries);

            return Ok(analysis);
        }
    }

}
