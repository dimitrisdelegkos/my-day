using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyDay.Core.Infrastructure.Concrete;
using MyDay.Core.Services.Concrete;
using NUnit.Framework;

namespace MyDay.Tests.UnitTests
{
    [TestFixture]
    public class OpenWeatherAPIOperationsServiceTests
    {
        private Mock<ILogger<HttpOperationsService>> _httpOperationsLoggerMock;
        private Mock<ILogger<OpenWeatherAPIOperationsService>> _openWeatherServiceLoggerMock;

        private Mock<IConfiguration> _configurationMock;
        private MemoryCache _memoryCacheMock;
        private Mock<IHttpClientFactory> _clientFactoryMock;

        private HttpOperationsService _httpOperationsMock;
        private OpenWeatherAPIOperationsService _openWeatherAPIOperationsService;

        [SetUp]
        public void Setup()
        {
            _httpOperationsLoggerMock = new Mock<ILogger<HttpOperationsService>>();
            _openWeatherServiceLoggerMock = new Mock<ILogger<OpenWeatherAPIOperationsService>>();

            _memoryCacheMock = new MemoryCache(new MemoryCacheOptions());
            _configurationMock = new Mock<IConfiguration>();
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _clientFactoryMock.Setup(x => x.CreateClient("open-weather-api")).Returns(
                new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(5)
                });

            _httpOperationsMock = new HttpOperationsService(_clientFactoryMock.Object, _httpOperationsLoggerMock.Object, _memoryCacheMock);

            _configurationMock.Setup(c => c.GetSection("OpenWeatherAPISettings:EndpointUrl").Value).Returns("https://api.openweathermap.org/data/3.0");
            _configurationMock.Setup(c => c.GetSection("OpenWeatherAPISettings:APIKey").Value).Returns("e3ad3a73b44b8d408affb968f28a98e4");
            _configurationMock.Setup(c => c.GetSection("OpenWeatherAPISettings:WeatherMeasurementUnits").Value).Returns("metric");

            _openWeatherAPIOperationsService = new OpenWeatherAPIOperationsService(_openWeatherServiceLoggerMock.Object, _configurationMock.Object, _httpOperationsMock);
        }

        [Test]
        public async Task GetTopHeadlines_WhenSuccessful_ReturnsSuccessWrapper()
        {
            bool expectedResult = true;
            var getTopHeadlinesResult = await _openWeatherAPIOperationsService.GetWeatherDailySummary((decimal)37.983810, (decimal)23.727539, DateTime.Today.ToString("yyyy-MM-dd"));
            Assert.That(getTopHeadlinesResult.IsSuccess, Is.EqualTo(expectedResult));
        }
    }
}
