using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyDay.Core.Application.Abstractions;
using MyDay.Core.Application.Models;
using MyDay.Core.Services.Abstractions;
using System.Globalization;
using System.Text.Json;

namespace MyDay.Core.Application.Concrete
{
    public class DailyTipsOperationsService : IDailyTipsOperations
    {
        private ILogger<DailyTipsOperationsService> _logger;
        private IConfiguration _configuration;

        private INewsOperations _newsOperationsService; 
        private IWeatherOperations _weatherOperationsService;

        public DailyTipsOperationsService(ILogger<DailyTipsOperationsService> logger,
            IConfiguration configuration,
            INewsOperations newsOperationsService,
            IWeatherOperations weatherOperationsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _newsOperationsService = newsOperationsService ?? throw new ArgumentNullException(nameof(newsOperationsService));
            _weatherOperationsService = weatherOperationsService ?? throw new ArgumentNullException(nameof(weatherOperationsService));
        }

        public async Task<DayTipsModel?> GetTipsOfToday(NewsFilteringCriteriaModel newsFilteringCriteria, 
            WeatherFilteringCriteriaModel weatherFilteringCriteria)
        {
            try
            {
                var dailyTips = new DayTipsModel();
                int topHeadlinesCount = _configuration.GetValue<int>("DailyTipsSettings:TopHeadlinesCount");

                #region Get News

                var dailyTopNewsHeadlinesResult = await _newsOperationsService.GetTopHeadlines(newsFilteringCriteria.Category, newsFilteringCriteria.Keyword);
                if (!dailyTopNewsHeadlinesResult.IsSuccess
                    || dailyTopNewsHeadlinesResult.TopHeadlines.TotalResults <= 0)
                {
                    _logger.LogTrace("No news could be retrieved for criteria: {NewsCriteria}", JsonSerializer.Serialize(newsFilteringCriteria));
                    dailyTips.News = null;
                }
                else
                {
                    dailyTips.News = dailyTopNewsHeadlinesResult.TopHeadlines.Articles.Take(topHeadlinesCount).Select(x => new ArticleModel
                    {
                        Author = x.Author,
                        Date = DateTime.Parse(x.PublishedAt, null, DateTimeStyles.RoundtripKind).ToString("dd/MM/yyyy"),
                        Source = x.Source.Name,
                        Title = x.Title,
                        Url = x.Url
                    });
                }

                #endregion

                #region Get Weather

                decimal latitude = weatherFilteringCriteria.Latitude == null
                    ? _configuration.GetValue<decimal>("DailyTipsSettings:LocationLatitude")
                    : weatherFilteringCriteria.Latitude.Value;
                decimal longitude = weatherFilteringCriteria.Longitude == null
                    ? _configuration.GetValue<decimal>("DailyTipsSettings:LocationLongitude")
                    : weatherFilteringCriteria.Longitude.Value;

                var dailyWeatherSummaryResult = await _weatherOperationsService.GetWeatherDailySummary(latitude, longitude, DateTime.Now.ToString("yyyy-MM-dd"));
                if (!dailyWeatherSummaryResult.IsSuccess)
                {
                    _logger.LogTrace("No weather data could be retrieved for criteria: {WeatherCriteria}", JsonSerializer.Serialize(weatherFilteringCriteria));
                    dailyTips.WeatherSummary = null;
                }
                else
                {
                    dailyTips.WeatherSummary = new WeatherSummaryModel
                    {
                        MaximumTemperature = dailyWeatherSummaryResult.WeatherDailySummary.Temperature.Max,
                        MinimumTemperature = dailyWeatherSummaryResult.WeatherDailySummary.Temperature.Min,
                        Humidity = dailyWeatherSummaryResult.WeatherDailySummary.Humidity.Afternoon,
                        MaxWindSpeed = dailyWeatherSummaryResult.WeatherDailySummary.Wind.Max.Speed
                    };
                }

                #endregion

                return dailyTips;
            }
            catch (Exception exception) 
            {
                _logger.LogError("An exception occurred during fetching daily tips: {Error}", exception.Message);
                return null;

            }
        }
    }
}
