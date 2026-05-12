using ProductionTimeAnalyzer.Dtos;
using ProductionTimeAnalyzer.Models;
using ProductionTimeAnalyzer.Models.Enums;

namespace ProductionTimeAnalyzer.Services
{
    /// <summary>
    /// Performs deterministic time analysis based on TimeEntry data.
    /// This service contains pure business logic and is AI-agnostic.
    /// </summary>
    public class TimeAnalysisService
    {
        public TimeAnalysisDto Analyze(IEnumerable<TimeEntry> entries)
        {
            double productionMinutes = 0;
            double downtimeMinutes = 0;

            foreach (var entry in entries)
            {
                if (entry.EndTime <= entry.StartTime)
                    continue;

                var minutes = (entry.EndTime - entry.StartTime).TotalMinutes;

                switch (entry.Status)
                {
                    case TimeEntryType.Production:
                        productionMinutes += minutes;
                        break;

                    case TimeEntryType.Downtime:
                        downtimeMinutes += minutes;
                        break;
                }
            }

            var total = productionMinutes + downtimeMinutes;

            return new TimeAnalysisDto
            {
                ProductionMinutes = (int)Math.Round(productionMinutes),
                DowntimeMinutes = (int)Math.Round(downtimeMinutes),
                DowntimePercentage = total == 0
                    ? 0
                    : downtimeMinutes / total * 100
            };
        }
    }
}
