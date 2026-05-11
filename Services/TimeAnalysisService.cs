using ProductionTimeAnalyzer.Dtos;
using ProductionTimeAnalyzer.Models;
using ProductionTimeAnalyzer.Models.Enums;


namespace ProductionTimeAnalyzer.Services
{
    public class TimeAnalysisService
    {
        // dies dient der Berechnung der Zeiten, also reine Fachlogik

        public TimeAnalysisDto Analyze(IEnumerable<TimeEntry> entries)
        {

            var productionMinutes = 0;
            var downtimeMinutes = 0;

            foreach (var entry in entries)
            {
                if (entry.EndTime <= entry.StartTime)
                    continue;

                var minutes = (int)(entry.EndTime - entry.StartTime).TotalMinutes;
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
                ProductionMinutes = productionMinutes,
                DowntimeMinutes = downtimeMinutes,
                DowntimePercentage = total == 0 ? 0 : (double)downtimeMinutes / total * 100
            };
        }
    }
}
