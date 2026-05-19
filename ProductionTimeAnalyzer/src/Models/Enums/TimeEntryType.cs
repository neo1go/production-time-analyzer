namespace ProductionTimeAnalyzer.Models.Enums
{
        public enum TimeEntryType
        {  // Timeentry kann genau einen dieser Zustände haben
            Setup,   //  0
            Production,// 1
            Downtime, // 2
            Rework // 3
        }
    }

