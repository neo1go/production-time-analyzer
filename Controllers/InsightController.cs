using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionTimeAnalyzer.Data;
using ProductionTimeAnalyzer.Dtos;
using ProductionTimeAnalyzer.Services;

namespace ProductionTimeAnalyzer.Controllers
{
    [ApiController]
    [Route("api/insights")]
    public class InsightController : ControllerBase
    {
        private readonly ProductionTimeAnalyzerContext _db;
        private readonly TimeAnalysisService _analysisService;
        private readonly ProductionInsightAgent _insightAgent;

        public InsightController(
            ProductionTimeAnalyzerContext db,
            TimeAnalysisService analysisService,
            ProductionInsightAgent insightAgent)
        {
            _db = db;
            _analysisService = analysisService;
            _insightAgent = insightAgent;
        }

        [HttpGet]
        public async Task<IActionResult> GetInsights(DateTime from, DateTime to)
        {
            // ✅ WICHTIGE VALIDIERUNG – DAS fehlte bisher
            if (from == default || to == default)
            {
                return BadRequest(
                    "Both 'from' and 'to' query parameters must be provided.");
            }

            if (to <= from)
            {
                return BadRequest(
                    "'to' must be greater than 'from'.");
            }

            // ✅ Zeit-Überlappung (korrekt)
            var entries = await _db.TimeEntries
                .Where(e => e.StartTime < to && e.EndTime > from)
                .ToListAsync();

            // ✅ Fachliche Analyse mit Zeit-Clipping
            var analysis = _analysisService.Analyze(entries, from, to);

            var machines = entries
                .GroupBy(e => e.Machine.Name)
                .Select(g => new MachineInsightDto{
            MachineName = g.Key,
                    Analysis = _analysisService.Analyze(g,from, to)
            }).ToList();

            // ✅ DTO für KI
            var input = new ProductionInsightInput
            {
                From = from,
                To = to,
                Analysis = analysis,
                Machines = machines
            };

            // ✅ KI-Analyse
            var insight = await _insightAgent.AnalyzeAsync(input);

            return Ok(insight);
        }
    }
}
