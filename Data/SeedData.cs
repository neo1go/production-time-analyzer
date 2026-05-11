using ProductionTimeAnalyzer.Models;
using ProductionTimeAnalyzer.Models.Enums;

// Dies sind Testdaten, die bei Projektstart erstellt oder abgefragt werden.
namespace ProductionTimeAnalyzer.Data
{
    public class SeedData
    {

        public static void Initialize(AppDbContext context)
        {
            // Falls DB noch nicht existiert (Sicherheit)
            context.Database.EnsureCreated();

            // Wenn schon Daten da sind → nichts tun
            if (context.Products.Any())
                return;

            // -------- Products --------
            var product1 = new Product
            {
                Name = "Gear Shaft",
                OrderNumber = "ORD-1001"
            };

            var product2 = new Product
            {
                Name = "Housing Block",
                OrderNumber = "ORD-1002"
            };

            context.Products.AddRange(product1, product2);

            // -------- Machines --------
            var machine1 = new Machine
            {
                Name = "CNC-01",
                Type = "CNC"
            };

            var machine2 = new Machine
            {
                Name = "Lathe-02",
                Type = "Lathe"
            };

            context.Machines.AddRange(machine1, machine2);

            context.SaveChanges();

            // -------- TimeEntries --------
            var baseTime = DateTime.Today.AddHours(6);

            var entries = new List<TimeEntry>
            {
                new TimeEntry
                {
                    ProductId = product1.Id,
                    MachineId = machine1.Id,
                    StartTime = baseTime,
                    EndTime = baseTime.AddMinutes(30),
                    Status = TimeEntryType.Setup
                },
                new TimeEntry
                {
                    ProductId = product1.Id,
                    MachineId = machine1.Id,
                    StartTime = baseTime.AddMinutes(30),
                    EndTime = baseTime.AddHours(2),
                    Status = TimeEntryType.Production
                },
                new TimeEntry
                {
                    ProductId = product1.Id,
                    MachineId = machine1.Id,
                    StartTime = baseTime.AddHours(2),
                    EndTime = baseTime.AddHours(2.5),
                    Status = TimeEntryType.Downtime
                },
                new TimeEntry
                {
                    ProductId = product2.Id,
                    MachineId = machine2.Id,
                    StartTime = baseTime,
                    EndTime = baseTime.AddMinutes(45),
                    Status = TimeEntryType.Setup
                },
                new TimeEntry
                {
                    ProductId = product2.Id,
                    MachineId = machine2.Id,
                    StartTime = baseTime.AddMinutes(45),
                    EndTime = baseTime.AddHours(1.5),
                    Status = TimeEntryType.Production
                }
            };

            context.TimeEntries.AddRange(entries);
            context.SaveChanges();

        }
    }
}
