namespace ProductionTimeAnalyzer.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? OrderNumber { get; set; }


        public ICollection<TimeEntry> TimeEntries { get; set; }
                = new List<TimeEntry>();

    }
}
