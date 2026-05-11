namespace ProductionTimeAnalyzer.Dtos
{
    public class TimeAnalysisDto
    {

        public int ProductionMinutes { get; set; }
        public int DowntimeMinutes { get; set; }
        public double DowntimePercentage { get; set; }
    }
}
