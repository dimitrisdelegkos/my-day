using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyDay.Core.Infrastructure.Concrete;
using MyDay.Core.Services.Concrete;
using NUnit.Framework;

[TestFixture]
public class NewsAPIOperationsServiceTests
{
    private Mock<ILogger<HttpOperationsService>> _httpOperationsLoggerMock;
    private Mock<ILogger<NewsAPIOperationsService>> _newsAPIServiceLoggerMock;

    private Mock<IConfiguration> _configurationMock;
    private MemoryCache _memoryCacheMock;
    private Mock<IHttpClientFactory> _clientFactoryMock;

    private HttpOperationsService _httpOperationsMock;
    private NewsAPIOperationsService _newsOperationsService;

    [SetUp]
    public void Setup()
    {
        _httpOperationsLoggerMock = new Mock<ILogger<HttpOperationsService>>();
        _newsAPIServiceLoggerMock = new Mock<ILogger<NewsAPIOperationsService>>();

        _memoryCacheMock = new MemoryCache(new MemoryCacheOptions());
        _configurationMock = new Mock<IConfiguration>();
        _clientFactoryMock = new Mock<IHttpClientFactory>();
        _clientFactoryMock.Setup(x => x.CreateClient("news-api")).Returns(
            new HttpClient 
            {
                Timeout = TimeSpan.FromSeconds(5)
            });
        
        _httpOperationsMock = new HttpOperationsService(_clientFactoryMock.Object, _httpOperationsLoggerMock.Object, _memoryCacheMock);

        _configurationMock.Setup(c => c.GetSection("NewsAPISettings:EndpointUrl").Value).Returns("https://newsapi.org/v2");
        _configurationMock.Setup(c => c.GetSection("NewsAPISettings:APIKey").Value).Returns("bcf66bde74b948fa9cf63049fd006f7c");

        _newsOperationsService = new NewsAPIOperationsService(_newsAPIServiceLoggerMock.Object, _configurationMock.Object, _httpOperationsMock);
    }

    [Test]
    public async Task GetTopHeadlines_WhenSuccessful_ReturnsSuccessWrapper()
    {
        bool expectedResult = true;
        var getTopHeadlinesResult = await _newsOperationsService.GetTopHeadlines("science", "nasa");
        Assert.That(getTopHeadlinesResult.IsSuccess, Is.EqualTo(expectedResult));
    }
}