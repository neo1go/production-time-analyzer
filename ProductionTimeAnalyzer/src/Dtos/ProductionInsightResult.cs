namespace ProductionTimeAnalyzer.Dtos
{
    public class ProductionInsightResult
    {

        public string Text { get; set; } = "";
        public string Summary { get; set; } = "";
        public List<string> Insights { get; set; } = [];
        public List<string> Warnings { get; set; } = [];

    }
}
