using ProductionTimeAnalyzer.Models.Enums;

namespace ProductionTimeAnalyzer.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MachineId { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public TimeEntryType Status { get; set; }


        
    }
}
