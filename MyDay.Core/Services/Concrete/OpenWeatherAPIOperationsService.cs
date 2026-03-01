using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyDay.Core.Infrastructure.Abstractions;
using MyDay.Core.Infrastructure.Models;
using MyDay.Core.Services.Abstractions;
using MyDay.Core.Services.Models.OpenWeatherAPI;
using System.Text.Json;

namespace MyDay.Core.Services.Concrete
{
    public class OpenWeatherAPIOperationsService : IWeatherOperations
    {
        private ILogger<OpenWeatherAPIOperationsService> _logger;
        private IConfiguration _configuration;
        private IHttpOperations _httpOperationsService;

        public OpenWeatherAPIOperationsService(ILogger<OpenWeatherAPIOperationsService> logger,
            IConfiguration configuration,
            IHttpOperations httpOperationsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpOperationsService = httpOperationsService ?? throw new ArgumentNullException(nameof(httpOperationsService));
        }

        public async Task<WeatherDailySummaryResponseWrapper> GetWeatherDailySummary(decimal latitude, decimal longitude, string date)
        {
            try
            {
                string baseEndpointUrl = _configuration.GetValue<string>("OpenWeatherAPISettings:EndpointUrl");
                string requestUrl = "/onecall/day_summary?lat={lat}&lon={lon}&date={date}&appid={appid}&units={units}";
                var requestQueryParameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("{appid}", _configuration.GetValue<string>("OpenWeatherAPISettings:APIKey")),
                    new KeyValuePair<string, string>("{lat}", latitude.ToString()),
                    new KeyValuePair<string, string>("{lon}", longitude.ToString()),
                    new KeyValuePair<string, string>("{date}", date),
                    new KeyValuePair<string, string>("{units}", _configuration.GetValue<string>("OpenWeatherAPISettings:WeatherMeasurementUnits"))
                }; 

                var requestModel = new HttpRequestModel
                {
                    ContentType = "application/json",
                    Headers = new List<KeyValuePair<string, string>>{},
                    HttpClient = "open-weather-api",
                    QueryParameters = requestQueryParameters,
                    TargetSystem = "OpenWeatherAPI",
                    Url = baseEndpointUrl + requestUrl
                };
                var getWeatherDailySummaryResult = await _httpOperationsService.Get(requestModel);

                if (getWeatherDailySummaryResult.HasError)
                {
                    return new WeatherDailySummaryResponseWrapper
                    {
                        IsSuccess = false,
                        Error = JsonSerializer.Deserialize<OpenWeatherAPIErrorResponseDto>(getWeatherDailySummaryResult.Payload)
                    };
                }
                else
                {
                    return new WeatherDailySummaryResponseWrapper
                    {
                        IsSuccess = true,
                        WeatherDailySummary = JsonSerializer.Deserialize<WeatherDailySummaryResponseDto>(getWeatherDailySummaryResult.Payload)
                    };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("An exception occurred during OpenWeatherAPI invocation: {Error}", exception.Message);
                return new WeatherDailySummaryResponseWrapper
                {
                    IsSuccess = false
                };
            }
        }
    }
}