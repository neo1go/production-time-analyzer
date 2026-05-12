using ProductionTimeAnalyzer.AI.Prompts;
using ProductionTimeAnalyzer.Dtos;
using System.Text.Json;

namespace ProductionTimeAnalyzer.Services
{
    public class ProductionInsightAgent
    {
        private readonly HttpClient _http;

        public ProductionInsightAgent(HttpClient http)
        {
            _http = http;
        }

        public async Task<ProductionInsightResult> AnalyzeAsync(
            ProductionInsightInput input)
        {

            var request = new
            {
                model = "qwen2.5-3b-instruct",
                messages = new[]
                        {
                new { role = "system", content = ProductionInsightPrompt.System },
                new { role = "user", content = BuildUserMessage(input) }
            },
                temperature = 0.3
            };

            var response = await _http.PostAsJsonAsync(
                "http://localhost:1234/v1/chat/completions",
                request);

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            var text = json
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return new ProductionInsightResult
            {
                Text = text ?? ""
            };
        }

        private static string BuildUserMessage(ProductionInsightInput input)
        {
            return $"""
            Analyze production performance from {input.From:d} to {input.To:d}.

            Production minutes: {input.Analysis.ProductionMinutes}
            Downtime minutes: {input.Analysis.DowntimeMinutes}
            Downtime percentage: {input.Analysis.DowntimePercentage:F1}%
            """;
        }

    }

}