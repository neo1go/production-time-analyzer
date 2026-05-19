using ProductionTimeAnalyzer.Models;
using ProductionTimeAnalyzer.Models.Enums;
using ProductionTimeAnalyzer.Services;
using Xunit;

namespace ProductionTimeAnalyzer.Tests
{
    public class ProductionTimeAnalyzerServiceTests()
    {
        [Fact]
        public void DummyTest1()
        {
            Assert.True(true);
        }

        [Fact]
        public void Analyze_WhenNoeEntries_ReturnsAllZeroValues()
        {
            // Arrange
            var service = new TimeAnalysisService();
            var entries = new List<TimeEntry>();

            var from = new DateTime(2004, 1, 1, 8, 0, 0);
            var to = new DateTime(2024, 1, 1, 16, 0, 0);

            // Act
            var result = service.Analyze(entries, from, to);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(0, result.ProductionMinutes);
            Assert.Equal(0, result.DowntimeMinutes);
            Assert.Equal(0, result.SetupMinutes);
            Assert.Equal(0, result.ReworkMinutes);
            Assert.Equal(0, result.UnclassifiedMinutes);

            Assert.Equal(0, result.DowntimePercentage);
        }

        [Fact]
        public void Analyze_WhenSingleProductionEntryWithinRange_ReturnsProductionMinutes()
        {
            // Arrange
            var service = new TimeAnalysisService();

            var from = new DateTime(2024, 1, 1, 8, 0, 0);
            var to = new DateTime(2024, 1, 1, 16, 0, 0);

            var entry = new TimeEntry
            {
                StartTime = new DateTime(2024, 1, 1, 9, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 10, 30, 0),
                Status = TimeEntryType.Production
            };

            var entries = new List<TimeEntry> { entry };

            // Act
            var result = service.Analyze(entries, from, to);

            // Assert
            Assert.Equal(90, result.ProductionMinutes);
            Assert.Equal(0, result.SetupMinutes);
            Assert.Equal(0, result.DowntimeMinutes);
            Assert.Equal(0, result.ReworkMinutes);
            Assert.Equal(0, result.UnclassifiedMinutes);
        }

        [Fact]
        public void Analyze_WhenEntryOutsideRange_IsIgnored()
        {
            // Arrange
            var service = new TimeAnalysisService();

            var from = new DateTime(2024, 1, 1, 8, 0, 0);
            var to = new DateTime(2024, 1, 1, 16, 0, 0);

            var entry = new TimeEntry
            {
                StartTime = new DateTime(2024, 1, 1, 6, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 7, 0, 0),
                Status = TimeEntryType.Production
            };

            var entries = new List<TimeEntry> { entry };

            // Act
            var result = service.Analyze(entries, from, to);

            // Assert
            Assert.Equal(0, result.ProductionMinutes);
            Assert.Equal(0, result.SetupMinutes);
            Assert.Equal(0, result.DowntimeMinutes);
            Assert.Equal(0, result.ReworkMinutes);
            Assert.Equal(0, result.UnclassifiedMinutes);
        }

        // Merged 3 Tests into one via 'Theory'
        [Theory]
        [InlineData(7,30,9,0,60)]
        [InlineData(15,0,16,30,60)]
        [InlineData(7,0,17,0,480)]
        public void Analyze_WhenEntryOverlapsRange_IsClippedCorrectly(int startHour, int startMinute,int endHour,int endMinute, int expectedMinutes)
        {
            var service = new TimeAnalysisService();

            var from = new DateTime(2024, 1, 1, 8, 0, 0);
            var to = new DateTime(2024, 1, 1, 16, 0, 0);

            var entry = new TimeEntry
            {
                StartTime = new DateTime(2024, 1, 1, startHour, startMinute, 0),
                EndTime = new DateTime(2024, 1, 1, endHour, endMinute, 0),
                Status = TimeEntryType.Production
            };

            // Act
            var result = service.Analyze(new[] {entry}, from, to);

            // Assert
            Assert.Equal(expectedMinutes, result.ProductionMinutes);
        }


        [Fact]
        public void Analyze_WhenMultipleProdcutionEntries_ReturnSumOfMinutes()
        {
            // Arrange
            var service = new TimeAnalysisService();

            var from = new DateTime(2024, 1, 1, 8, 0, 0);
            var to = new DateTime(2024, 1, 1, 16, 0, 0);

            var entries = new List<TimeEntry>
            {
             new TimeEntry
                {
                    StartTime = new DateTime(2024, 1, 1, 9, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 10, 0, 0),
                    Status = Models.Enums.TimeEntryType.Production
                },
             new TimeEntry
             {
                 StartTime = new DateTime(2024, 1, 1, 11, 0, 0),
                    EndTime = new DateTime(2024, 1, 1, 12, 30, 0),
                    Status = TimeEntryType.Production
             }

                };
            // Act
            var result = service.Analyze(entries, from, to);

            // Assert
            Assert.Equal(150, result.ProductionMinutes);
        }

        [Fact]
        public void Analyze_WhenEntryHAsUnkonwnStatus_IsCountedAsUnclassified()
        {

            var service = new TimeAnalysisService();

            var from = new DateTime(2024, 1, 1, 8, 0, 0);
            var to = new DateTime(2024, 1, 1, 16, 0, 0);

            var entry = new TimeEntry
            {
                StartTime = new DateTime(2024, 1, 1, 10, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 11, 0, 0),
                Status = (TimeEntryType)999   // bewusst ungültig
            };

            // Act
            var result = service.Analyze(new[] { entry }, from, to);

            // Assert
            Assert.Equal(60, result.UnclassifiedMinutes);
        }

        [Fact]
        public void Analyze_WhenDowntimePresent_CalculatesDowntimePercentage()
        {
            var service = new TimeAnalysisService();

            var from = new DateTime(2024, 1, 1, 8, 0, 0);
            var to = new DateTime(2024, 1, 1, 16, 0, 0);

            var entries = new List<TimeEntry>
    {
        new TimeEntry
        {
            StartTime = new DateTime(2024, 1, 1, 9, 0, 0),
            EndTime   = new DateTime(2024, 1, 1, 11, 0, 0),
            Status    = TimeEntryType.Production
        },
        new TimeEntry
        {
            StartTime = new DateTime(2024, 1, 1, 11, 0, 0),
            EndTime   = new DateTime(2024, 1, 1, 12, 0, 0),
            Status    = TimeEntryType.Downtime
        }
    };

            var result = service.Analyze(entries, from, to);

            Assert.Equal(60, result.DowntimeMinutes);
            Assert.InRange(result.DowntimePercentage, 33.3, 33.4);
        }

        [Fact]
        public void Analyze_WhenEntryHasNegativeDuration_IsIgnored()
        {
            var service = new TimeAnalysisService();

            var from = new DateTime(2024, 1, 1, 8, 0, 0);
            var to = new DateTime(2024, 1, 1, 16, 0, 0);

            var entry = new TimeEntry
            {
                StartTime = new DateTime(2024, 1, 1, 12, 0, 0),
                EndTime = new DateTime(2024, 1, 1, 10, 0, 0), // negativ
                Status = TimeEntryType.Production
            };

            var result = service.Analyze(new[] { entry }, from, to);

            Assert.Equal(0, result.ProductionMinutes);
        }


    }
}