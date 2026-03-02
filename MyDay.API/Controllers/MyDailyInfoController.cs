using Microsoft.AspNetCore.Mvc;
using MyDay.API.Enums;
using MyDay.API.Models;
using MyDay.Core;
using MyDay.Core.Application.Abstractions;
using MyDay.Core.Application.Models.Music;
using MyDay.Core.Application.Models.News;
using MyDay.Core.Application.Models.Weather;

namespace MyDay.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyDailyInfoController : ControllerBase
    {

        private readonly ILogger<MyDailyInfoController> _logger;
        private readonly IDailyTipsOperations _dailyTipsOperationsService;

        public MyDailyInfoController(ILogger<MyDailyInfoController> logger,
            IDailyTipsOperations dailyTipsOperationsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dailyTipsOperationsService = dailyTipsOperationsService ?? throw new ArgumentNullException(nameof(dailyTipsOperationsService));
        }

        /// <summary>
        /// Accepts a set of criteria and return top news headlines, a group of proposed music playlist and the weather report for today
        /// </summary>
        /// <param name="musicKeyword">A keyword for fetching related music playlists</param>
        /// <param name="newsCategory">A category for filtering news articles</param>
        /// <param name="newsKeyword">A keyword for filtering news articles</param>
        /// <param name="weatherLocationLatitude">The latitude of the location used for the weather daily summary. If null, the latitude of Athens is used.</param>
        /// <param name="weatherLocationlongitude">The longitude of the location used for the weather daily summary. If null, the longitude of Athens is used.</param>
        /// <returns><see cref="DayTipsResponseDto"/></returns>
        /// <response code="200">The request was processed successfully</response>
        /// <response code="400">Bad request. Response should describe the cause of failure</response>
        /// <response code="500">Internal Server Error. Response should describe the cause of failure</response>
        [ProducesResponseType(typeof(DayTipsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DayTipsResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DayTipsResponseDto), StatusCodes.Status500InternalServerError)]
        [HttpGet(Name = "GetDayTips")]
        public async Task<IActionResult> GetDayTips(string musicKeyword, 
            string newsCategory, string newsKeyword = "",
            decimal? weatherLocationLatitude = null, decimal? weatherLocationlongitude = null)
        {
            try
            {
                #region Validation

                var validationErrors = new List<KeyValuePair<string, string>>();
                if (string.IsNullOrWhiteSpace(musicKeyword))
                {
                    validationErrors.Add(new KeyValuePair<string, string>(Errors.InvalidValue, "Please assing a value for parameter musicKeyword"));
                }
                if (!string.IsNullOrWhiteSpace(newsCategory) 
                    && !Enum.TryParse(newsCategory, out NewsArticleCategory newsArticleCategory))
                {
                    validationErrors.Add(new KeyValuePair<string, string>(Errors.InvalidValue, "Please assing a valid value for parameter newsCategory"));
                }
                if(weatherLocationLatitude != null)
                {
                    if(weatherLocationLatitude.Value > 90 
                        || weatherLocationLatitude.Value < -90)
                    {
                        validationErrors.Add(new KeyValuePair<string, string>(Errors.InvalidValue, "Please assing a valid value for parameter weatherLocationLatitude"));
                    }
                }
                if (weatherLocationlongitude != null)
                {
                    if (weatherLocationlongitude.Value > 180
                        || weatherLocationlongitude.Value < -180)
                    {
                        validationErrors.Add(new KeyValuePair<string, string>(Errors.InvalidValue, "Please assing a valid value for parameter weatherLocationlongitude"));
                    }
                }

                if (validationErrors.Count() > 0)
                {
                    return StatusCode(
                       StatusCodes.Status400BadRequest,
                       new DayTipsResponseDto
                       {
                           Status = Status.FAILURE,
                           Errors = validationErrors
                       });
                }

                #endregion

                var getTipOfTodayResult = await _dailyTipsOperationsService.GetTipsOfToday(
                        new NewsFilteringCriteriaModel
                        {
                            Category = newsCategory,
                            Keyword = newsKeyword,
                        },
                        new WeatherFilteringCriteriaModel
                        {  
                            Latitude = weatherLocationLatitude,
                            Longitude = weatherLocationlongitude
                        },
                        new MusicFilteringCriteriaModel
                        {
                            Keyword = musicKeyword
                        }
                );
                if (getTipOfTodayResult == null)
                {
                    return StatusCode(
                       StatusCodes.Status500InternalServerError,
                       new DayTipsResponseDto
                       {
                           Status = Status.FAILURE,
                           Errors = new Dictionary<string, string>() { [Errors.Generic] = "An error occurred during fetching the tips of today" }
                       });
                }

                bool foundNewsData = getTipOfTodayResult?.News.Count() > 0;
                bool foundWeatherData = getTipOfTodayResult?.WeatherSummary != null;
                bool foundMusicData = getTipOfTodayResult?.Playlists.Count() > 0; 

                return Ok(new DayTipsResponseDto
                {
                    Status = Status.SUCCESS,
                    NewsToRead = new NewsDto 
                    {
                       ResultMessage = !foundNewsData ? "Could not retrieve top headlines of today, please try again." : $"Found {getTipOfTodayResult?.News.Count()} articles.",
                       TopArticleHeadlines = foundNewsData
                        ? getTipOfTodayResult?.News.Select(x=> new TopArticleHeadlineDto
                        {
                            Author = x.Author,
                            Date = x.Date,
                            Source = x.Source,
                            Title = x.Title,
                            Url = x.Url
                        })
                        : Enumerable.Empty<TopArticleHeadlineDto>()
                    },
                    WeatherPrognosis = new WeatherDailyReportDto
                    {
                        ResultMessage = !foundWeatherData ? "Could not retrieve the weather prognosis of today, please try again." : "Found weather data for today!",
                        WeatherDailySummary = new WeatherDailySummaryDto
                        {
                            MaximumTemperature = getTipOfTodayResult?.WeatherSummary?.MaximumTemperature ?? 0,
                            MinimumTemperature = getTipOfTodayResult?.WeatherSummary?.MinimumTemperature ?? 0,
                            Humidity = getTipOfTodayResult?.WeatherSummary?.Humidity ?? 0,
                            MaxWindSpeed = getTipOfTodayResult?.WeatherSummary?.MaxWindSpeed ?? 0
                        }
                    },
                    MusicForToday = new MusicDto
                    {
                        ResultMessage = !foundMusicData ? "Could not retrieve any music for today, please try again." : $"Nice tunes match with the keyword {musicKeyword}!",
                        TopPlaylists  = foundMusicData
                        ? getTipOfTodayResult?.Playlists.Select(x => new PlaylistDto
                        {
                            Title = x.Name,
                            Summary = x.Description,
                            Tracks = x.Songs,
                            Url = x.Link
                        })
                        : Enumerable.Empty<PlaylistDto>()
                    }
                });
            }
            catch (Exception exception) 
            {
                _logger.LogError(exception.Message);
                return StatusCode(
                       StatusCodes.Status500InternalServerError,
                       new DayTipsResponseDto
                       {
                           Status = Status.FAILURE,
                           Errors = new Dictionary<string, string>() { [Errors.Generic] = exception.Message }
                       });
            }
        }
    }
}
