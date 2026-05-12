namespace ProductionTimeAnalyzer.Dtos
{
    /// <summary>
    /// Aggregated production time analysis for a given period.
    /// All values are expressed in minutes and are already clipped
    /// to the requested analysis timeframe.
    /// </summary>
    public class TimeAnalysisDto
    {
        // ✅ Kernzeiten
        public int ProductionMinutes { get; set; }
        public int DowntimeMinutes { get; set; }
        public int SetupMinutes { get; set; }
        public int ReworkMinutes { get; set; }

        // ✅ Kennzahl auf Basis ALLER Zeitarten
        public double DowntimePercentage { get; set; }
    }
}