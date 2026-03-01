using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyDay.Integrations.Application.Abstractions;
using MyDay.Integrations.Application.Models.NewsAPI;
using MyDay.Integrations.Infrastructure.Abstractions;
using MyDay.Integrations.Infrastructure.Models;
using System.Text.Json;

namespace MyDay.Integrations.Application.Concrete
{
    public class NewsAPIOperationsService : INewsOperations
    {
        private ILogger<NewsAPIOperationsService> _logger;
        private IConfiguration _configuration;
        private IHttpOperations _httpOperations;

        public NewsAPIOperationsService(ILogger<NewsAPIOperationsService> logger, 
            IConfiguration configuration,
            IHttpOperations httpOperations)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpOperations = httpOperations ?? throw new ArgumentNullException(nameof(httpOperations));
        }

        public async Task<TopHeadlinesResponseWrapper> GetTopHeadlines(string category, string keyword)
        {
            try
            {
                var requestQueryParameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("apiKey", _configuration.GetValue<string>("NewsAPISettings:APIKey"))
                };
                if (!String.IsNullOrWhiteSpace(category))
                    requestQueryParameters.Add(new KeyValuePair<string, string>("category", category));
                if (!String.IsNullOrWhiteSpace(keyword))
                    requestQueryParameters.Add(new KeyValuePair<string, string>("q", keyword));

                var requestModel = new HttpRequestModel
                {
                    ContentType = "application/json",
                    Headers = new List<KeyValuePair<string, string>> {},
                    HttpClient = "news-api",
                    QueryParameters = requestQueryParameters,
                    TargetSystem = "NewsAPI",
                    Url = "/top-headlines"
                };
                var getTopHeadLinesResult = await _httpOperations.Get(requestModel);

                if (getTopHeadLinesResult.HasError)
                {
                    return new TopHeadlinesResponseWrapper
                    {
                        IsSuccess = false,
                        Error = JsonSerializer.Deserialize<ErrorResponseDto>(getTopHeadLinesResult.Payload)
                    };
                }
                else
                {
                    return new TopHeadlinesResponseWrapper
                    {
                        IsSuccess = true,
                        TopHeadlines = JsonSerializer.Deserialize<TopHeadlinesResponseDto>(getTopHeadLinesResult.Payload)
                    };
                }
            }
            catch(Exception exception)
            {
                _logger.LogError("An exception occurred during NewsAPI invocation: {Error}", exception.Message);
                return new TopHeadlinesResponseWrapper
                {
                    IsSuccess = false
                };
            }
        }
    }
} 