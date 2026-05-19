namespace ProductionTimeAnalyzer.Dtos
{
    public class TimeEntryDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string MachineName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }


}
