using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyDay.Core.Infrastructure.Abstractions;
using MyDay.Core.Infrastructure.Models;
using MyDay.Core.Services.Abstractions;
using MyDay.Core.Services.Models.NewsAPI.Common;
using MyDay.Core.Services.Models.NewsAPI.TopHeadlines;
using System.Text.Json;

namespace MyDay.Core.Services.Concrete
{
    public class NewsAPIOperationsService : INewsOperations
    {
        private ILogger<NewsAPIOperationsService> _logger;
        private IConfiguration _configuration;
        private IHttpOperations _httpOperationsService;

        public NewsAPIOperationsService(ILogger<NewsAPIOperationsService> logger, 
            IConfiguration configuration,
            IHttpOperations httpOperationsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpOperationsService = httpOperationsService ?? throw new ArgumentNullException(nameof(httpOperationsService));
        }

        public async Task<TopHeadlinesResponseWrapper> GetTopHeadlines(string category, string keyword)
        {
            try
            {
                string baseEndpointUrl = _configuration.GetValue<string>("NewsAPISettings:EndpointUrl");
                string requestUrl = "/top-headlines?category={category}&apiKey={apiKey}";
                var requestQueryParameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("{apiKey}", _configuration.GetValue<string>("NewsAPISettings:APIKey"))
                };

                if (!string.IsNullOrWhiteSpace(category))
                {
                    requestQueryParameters.Add(new KeyValuePair<string, string>("{category}", category));
                }
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    requestQueryParameters.Add(new KeyValuePair<string, string>("{q}", keyword));
                    requestUrl = "/top-headlines?category={category}&q={q}&apiKey={apiKey}";
                }

                var requestModel = new HttpRequestModel
                {
                    ContentType = "application/json",
                    Headers = new List<KeyValuePair<string, string>> 
                    {
                        new KeyValuePair<string, string>("User-Agent", "MyDay API")
                    },
                    HttpClient = "news-api",
                    QueryParameters = requestQueryParameters,
                    TargetSystem = "NewsAPI",
                    Url = baseEndpointUrl + requestUrl
                };
                var getTopHeadLinesResult = await _httpOperationsService.Get(requestModel);

                if (getTopHeadLinesResult.HasError)
                {
                    return new TopHeadlinesResponseWrapper
                    {
                        IsSuccess = false,
                        Error = JsonSerializer.Deserialize<NewsAPIErrorResponseDto>(getTopHeadLinesResult.Payload)
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
                _logger.LogError("An exception occurred during NewsAPI-GetTopHeadlines invocation: {Error}", exception.Message);
                return new TopHeadlinesResponseWrapper
                {
                    IsSuccess = false
                };
            }
        }
    }
} 