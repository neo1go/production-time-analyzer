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
            // 1️⃣ TimeEntries sauber über EF Core aus der DB holen
            var entries = await _db.TimeEntries
                .Where(e => e.StartTime >= from && e.EndTime <= to)
                .ToListAsync();

            // 2️⃣ Fachliche Analyse (reine Business-Logik, KEINE KI)
            var analysis = _analysisService.Analyze(entries);

            // 3️⃣ DTO für den KI-Agenten bauen
            var input = new ProductionInsightInput
            {
                From = from,
                To = to,
                Analysis = analysis
            };

            // 4️⃣ KI-Insight erzeugen (read-only, erklärend)
            var insight = await _insightAgent.AnalyzeAsync(input);

            return Ok(insight);
        }
    }
}