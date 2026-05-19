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
        public async Task<IActionResult> GetInsights(
            DateTime from,
            DateTime to,
            int? machineId)
        {
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

            // ✅ Zeitraum normalisieren
            var effectiveFrom = from.Date;
            var effectiveTo = to.Date.AddDays(1);

            // ✅ Basis-Query
            var query = _db.TimeEntries
                .Include(e => e.Machine)
                .Where(e => e.StartTime < effectiveTo &&
                            e.EndTime > effectiveFrom);

            // ✅ Maschinenfilter (DER entscheidende Punkt)
            if (machineId.HasValue)
            {
                query = query.Where(e => e.MachineId == machineId.Value);
            }

            var entries = await query.ToListAsync();

            // ✅ Gesamtauswertung (jetzt korrekt gefiltert)
            var overallAnalysis =
                _analysisService.Analyze(entries, effectiveFrom, effectiveTo);

            // ✅ Pro Maschine (nur relevant bei Gesamtansicht)
            var machines = machineId.HasValue
                ? new List<MachineInsightDto>()   // bewusst leer
                : entries
                    .GroupBy(e => e.Machine)
                    .Select(g => new MachineInsightDto
                    {
                        MachineName = g.Key?.Name ?? "(unknown)",
                        Analysis = _analysisService.Analyze(
                            g,
                            effectiveFrom,
                            effectiveTo)
                    })
                    .ToList();

            // ✅ DTO für KI
            var input = new ProductionInsightInput
            {
                From = effectiveFrom,
                To = effectiveTo,
                Analysis = overallAnalysis,
                Machines = machines
            };

            // ✅ KI-Analyse
            var insight = await _insightAgent.AnalyzeAsync(input);

            return Ok(insight);
        }
    }
}
