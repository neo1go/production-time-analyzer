namespace ProductionTimeAnalyzer.Dtos
{
    public class MachineInsightDto
    {

        public string MachineName { get; set; } = "";
        public TimeAnalysisDto Analysis { get; set; } = null!;

    }
}
