using ProductionTimeAnalyzer.Models.Enums;

namespace ProductionTimeAnalyzer.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }
        //DB Beziehungen
        public int ProductId { get; set; }
        public int MachineId { get; set; }

        //Zeiger im Code für EF Core (navigation properties)
        public Product Product { get; set; } = null!;
        public Machine Machine { get; set; } = null!;

        public DateTime StartTime {  get; set; }
        public DateTime EndTime { get; set; }
        public TimeEntryType Status { get; set; }

    }
}
