using ProductionTimeAnalyzer.AI.Prompts;
using ProductionTimeAnalyzer.Controllers;
using ProductionTimeAnalyzer.Dtos;
using System.Text.Json;
using Microsoft.Extensions.Logging;

// KI Agent Verbindung
namespace ProductionTimeAnalyzer.Services
{
    public class ProductionInsightAgent
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;
        private readonly ILogger<ProductionInsightAgent> _logger;
        public ProductionInsightAgent(HttpClient http, IConfiguration config, ILogger<ProductionInsightAgent> logger)
        {
            _http = http;
            _config = config;
            _logger = logger;
        }
        // Dies ist der LMStudio Task 
        /*
              public async Task<ProductionInsightResult> AnalyzeAsync(
                  ProductionInsightInput input)
              {
                  var userMessage = BuildUserMessage(input);
                  _logger.LogInformation("=== PROMPT AN KI ===\n{UserMessage}", userMessage);

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
                      $"{_config["Llm:BaseUrl"]}/v1/chat/completions",
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
              */

        // dies ist der Ollama Task
        public async Task<ProductionInsightResult> AnalyzeAsync(
    ProductionInsightInput input)
        {
            var userMessage = BuildUserMessage(input);
            _logger.LogInformation("=== PROMPT AN KI ===\n{UserMessage}", userMessage);

            var request = new
            {
                model = "gemma3n:e2b ",   // Ollama-Modellname
                messages = new[]
                {
            new { role = "system", content = ProductionInsightPrompt.System },
            new { role = "user", content = userMessage }
        },
                stream = false,                  // wichtig: sonst erhält man Streaming-Chunks,also stückweise Antwort.
                options = new
                {
                    temperature = 0.3
                }
            };

            var response = await _http.PostAsJsonAsync(
                $"{_config["Llm:BaseUrl"]}/api/chat",
                request);

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            var text = json
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
            var machines = string.Join("\n", input.Machines.Select(m => $"{m.MachineName}: Prod={m.Analysis.ProductionMinutes} min, Down={m.Analysis.DowntimeMinutes} min"));
            return $"""
            Analyze production performance from {input.From:d} to {input.To:d}.

            Production minutes: {input.Analysis.ProductionMinutes}
            Downtime minutes: {input.Analysis.DowntimeMinutes}
            Downtime percentage: {input.Analysis.DowntimePercentage:F1}%
            UclassifiedMinutes: {input.Analysis.UnclassifiedMinutes}
            """;
        }

    }

}