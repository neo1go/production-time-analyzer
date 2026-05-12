using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionTimeAnalyzer.Data;
using ProductionTimeAnalyzer.Dtos;
using ProductionTimeAnalyzer.Services;

namespace ProductionTimeAnalyzer.Controllers
{
    [ApiController]
    [Route("api/analysis")]
    public class AnalysisController : ControllerBase
    {
        private readonly ProductionTimeAnalyzerContext _context;
        private readonly TimeAnalysisService _analysisService;

        public AnalysisController(
            ProductionTimeAnalyzerContext context,
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
            // ✅ Fallback-Zeitraum festlegen (wichtig!)
            var from = startDate ?? DateTime.MinValue;
            var to = endDate ?? DateTime.MaxValue;

            if (to <= from)
                return BadRequest("endDate must be greater than startDate.");

            // ✅ Grundabfrage
            var query = _context.TimeEntries.AsQueryable();

            // ✅ KORREKTE Zeit-Überlappung (nicht vollständig innerhalb!)
            query = query.Where(t =>
                t.StartTime < to &&
                t.EndTime > from);

            if (machineId.HasValue)
                query = query.Where(t => t.MachineId == machineId.Value);

            var entries = await query.ToListAsync();

            // ✅ Analyse mit Zeit-Clipping
            var analysis = _analysisService.Analyze(entries, from, to);

            return Ok(analysis);
        }
    }
}