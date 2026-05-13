using ProductionTimeAnalyzer.Dtos;
using ProductionTimeAnalyzer.Models;
using ProductionTimeAnalyzer.Models.Enums;

namespace ProductionTimeAnalyzer.Services
{
    public class TimeAnalysisService
    {
        public TimeAnalysisDto Analyze(
            IEnumerable<TimeEntry> entries,
            DateTime from,
            DateTime to)
        {
            double productionMinutes = 0;
            double downtimeMinutes = 0;
            double setupMinutes = 0;
            double reworkMinutes = 0;
            double unclassifiedMinutes = 0;

            foreach (var entry in entries)
            {
                // ✅ Zeit-Überlappung korrekt clippen
                var effectiveStart = entry.StartTime < from
                    ? from
                    : entry.StartTime;

                var effectiveEnd = entry.EndTime > to
                    ? to
                    : entry.EndTime;

                if (effectiveEnd <= effectiveStart)
                    continue;

                var minutes = (effectiveEnd - effectiveStart).TotalMinutes;

                switch (entry.Status)
                {
                    case TimeEntryType.Production:
                        productionMinutes += minutes;
                        break;

                    case TimeEntryType.Downtime:
                        downtimeMinutes += minutes;
                        break;

                    case TimeEntryType.Setup:
                        setupMinutes += minutes;
                        break;

                    case TimeEntryType.Rework:
                        reworkMinutes += minutes;
                        break;

                    default:
                        unclassifiedMinutes += minutes;
                        break;
                }
            }

            // ✅ Gesamtzeit = ALLE Zeitarten
            var totalMinutes =
                productionMinutes +
                downtimeMinutes +
                setupMinutes +
                reworkMinutes +
                unclassifiedMinutes;

            return new TimeAnalysisDto
            {
                ProductionMinutes = (int)Math.Round(productionMinutes),
                DowntimeMinutes = (int)Math.Round(downtimeMinutes),
                SetupMinutes = (int)Math.Round(setupMinutes),
                ReworkMinutes = (int)Math.Round(reworkMinutes),
                UnclassifiedMinutes = (int)Math.Round(unclassifiedMinutes),  // sollten faktisch 0 sein

                // ✅ Prozent jetzt fachlich korrekt
                DowntimePercentage = totalMinutes == 0
                    ? 0
                    : downtimeMinutes / totalMinutes * 100
            };
        }
    }
}