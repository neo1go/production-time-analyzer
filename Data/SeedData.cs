using ProductionTimeAnalyzer.Models;
using ProductionTimeAnalyzer.Models.Enums;

namespace ProductionTimeAnalyzer.Data
{
    public class SeedData
    {
        public static void Initialize(ProductionTimeAnalyzerContext context)
        {
            context.Database.EnsureCreated();

            // --------------------
            // PRODUCTS
            // --------------------
            var products = new[]
            {
                new Product { Name = "Gear Shaft",    OrderNumber = "ORD-1001" },
                new Product { Name = "Housing Block", OrderNumber = "ORD-1002" },
                new Product { Name = "Drive Plate",   OrderNumber = "ORD-1003" },
                new Product { Name = "Bearing Unit",  OrderNumber = "ORD-1004" }
            };

            foreach (var p in products)
            {
                if (!context.Products.Any(x => x.OrderNumber == p.OrderNumber))
                    context.Products.Add(p);
            }

            // --------------------
            // MACHINES
            // --------------------
            var machines = new[]
            {
                new Machine { Name = "CNC-01",   Type = "CNC" },
                new Machine { Name = "Lathe-02", Type = "Lathe" },
                new Machine { Name = "Mill-03",  Type = "Milling" }
            };

            foreach (var m in machines)
            {
                if (!context.Machines.Any(x => x.Name == m.Name))
                    context.Machines.Add(m);
            }

            context.SaveChanges();

            // --------------------
            // TIME ENTRIES
            // (nur einmal!)
            // --------------------
            if (context.TimeEntries.Any())
                return;

            var productMap = context.Products
                .ToDictionary(p => p.OrderNumber);

            var machineMap = context.Machines
                .ToDictionary(m => m.Name);

            var baseTime = DateTime.Today.AddHours(6);

            var entries = new List<TimeEntry>
            {
                // Gear Shaft – CNC
                new TimeEntry
                {
                    ProductId = productMap["ORD-1001"].Id,
                    MachineId = machineMap["CNC-01"].Id,
                    StartTime = baseTime,
                    EndTime = baseTime.AddMinutes(20),
                    Status = TimeEntryType.Setup
                },
                new TimeEntry
                {
                    ProductId = productMap["ORD-1001"].Id,
                    MachineId = machineMap["CNC-01"].Id,
                    StartTime = baseTime.AddMinutes(20),
                    EndTime = baseTime.AddHours(2),
                    Status = TimeEntryType.Production
                },
                new TimeEntry
                {
                    ProductId = productMap["ORD-1001"].Id,
                    MachineId = machineMap["CNC-01"].Id,
                    StartTime = baseTime.AddHours(2),
                    EndTime = baseTime.AddHours(2.25),
                    Status = TimeEntryType.Downtime
                },

                // Housing Block – Lathe
                new TimeEntry
                {
                    ProductId = productMap["ORD-1002"].Id,
                    MachineId = machineMap["Lathe-02"].Id,
                    StartTime = baseTime,
                    EndTime = baseTime.AddMinutes(30),
                    Status = TimeEntryType.Setup
                },
                new TimeEntry
                {
                    ProductId = productMap["ORD-1002"].Id,
                    MachineId = machineMap["Lathe-02"].Id,
                    StartTime = baseTime.AddMinutes(30),
                    EndTime = baseTime.AddHours(1.75),
                    Status = TimeEntryType.Production
                },

                // Drive Plate – Mill
                new TimeEntry
                {
                    ProductId = productMap["ORD-1003"].Id,
                    MachineId = machineMap["Mill-03"].Id,
                    StartTime = baseTime.AddHours(1),
                    EndTime = baseTime.AddMinutes(90),
                    Status = TimeEntryType.Setup
                },
                new TimeEntry
                {
                    ProductId = productMap["ORD-1003"].Id,
                    MachineId = machineMap["Mill-03"].Id,
                    StartTime = baseTime.AddMinutes(90),
                    EndTime = baseTime.AddHours(3),
                    Status = TimeEntryType.Production
                },
                new TimeEntry
                {
                    ProductId = productMap["ORD-1003"].Id,
                    MachineId = machineMap["Mill-03"].Id,
                    StartTime = baseTime.AddHours(3),
                    EndTime = baseTime.AddHours(3.5),
                    Status = TimeEntryType.Rework
                },

                // Bearing Unit – CNC
                new TimeEntry
                {
                    ProductId = productMap["ORD-1004"].Id,
                    MachineId = machineMap["CNC-01"].Id,
                    StartTime = baseTime.AddHours(2),
                    EndTime = baseTime.AddHours(2.5),
                    Status = TimeEntryType.Setup
                },
                new TimeEntry
                {
                    ProductId = productMap["ORD-1004"].Id,
                    MachineId = machineMap["CNC-01"].Id,
                    StartTime = baseTime.AddHours(2.5),
                    EndTime = baseTime.AddHours(4),
                    Status = TimeEntryType.Production
                }
            };

            context.TimeEntries.AddRange(entries);
            context.SaveChanges();
        }
    }
}