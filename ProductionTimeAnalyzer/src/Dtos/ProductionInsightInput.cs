namespace ProductionTimeAnalyzer.Dtos
{
    public class ProductionInsightInput
    {

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public List<MachineInsightDto> Machines { get; set; } = [];
        public TimeAnalysisDto? Analysis { get; set; } = null;
    }
}
