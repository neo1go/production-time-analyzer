using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using ProductionTimeAnalyzer.Dtos;
using ProductionTimeAnalyzer.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;


namespace ProductionTimeAnalyzer.Tests
{
    public class ProductionInsightAgentTests
    {
        [Fact]
        public async Task AnalyzeAsync_WhenValidResponse_ReturnsInsightText()
        {
            // Arrange
            var fakeJson = new
            {
                message = new
                {
                    content = "Production looks stable with minor downtime."
                }
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(fakeJson),
                    Encoding.UTF8,
                    "application/json")
            };

            var handler = new FakeHttpMessageHandler(response);
            var httpClient = new HttpClient(handler);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Llm:BaseUrl"] = "http://fake-ollama"
                })
                .Build();

            var logger = NullLogger<ProductionInsightAgent>.Instance;

            var agent = new ProductionInsightAgent(httpClient, config, logger);

            var input = new ProductionInsightInput
            {
                Analysis = new TimeAnalysisDto
                {
                    ProductionMinutes = 120,
                    DowntimeMinutes = 30,
                    DowntimePercentage = 20
                },
                From = DateTime.Today,
                To = DateTime.Today.AddDays(1),
                Machines = new List<MachineInsightDto>()
            };

            // Act
            var result = await agent.AnalyzeAsync(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Production looks stable with minor downtime.", result.Text);
        }

        [Fact]
        public void BuildUserMessage_IncludesDowntimeAndProduction()
        {
            var input = new ProductionInsightInput
            {
                Analysis = new TimeAnalysisDto
                {
                    ProductionMinutes = 100,
                    DowntimeMinutes = 50,
                    DowntimePercentage = 33.3
                },
                From = new DateTime(2024, 1, 1),
                To = new DateTime(2024, 1, 2),
                Machines = new List<MachineInsightDto>()
            };

            var message = ProductionInsightAgent.BuildUserMessage(input);

            Assert.Contains("Production minutes: 100", message);
            Assert.Contains("Downtime minutes: 50", message);
        }
    }
}
