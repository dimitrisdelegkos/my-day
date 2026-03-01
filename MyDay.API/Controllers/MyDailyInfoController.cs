using Microsoft.AspNetCore.Mvc;
using MyDay.API.Enums;
using MyDay.API.Models;
using MyDay.Core;
using MyDay.Core.Application.Abstractions;
using MyDay.Core.Application.Models;

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
        /// Accepts a set of criteria and return top news headlines, a proposed music playlist and the weather report for today
        /// </summary>
        /// <param name="newsCategory">A category for filtering news articles</param>
        /// <param name="newsKeyword">A keyword for filtering news articles</param>
        /// <returns><see cref="DayTipsResponseDto"/></returns>
        /// <response code="200">The request was processed successfully</response>
        /// <response code="400">Bad request. Response should describe the cause of failure</response>
        /// <response code="500">Internal Server Error. Response should describe the cause of failure</response>
        [ProducesResponseType(typeof(DayTipsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DayTipsResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DayTipsResponseDto), StatusCodes.Status500InternalServerError)]
        [HttpGet(Name = "GetDayTips")]
        //[ResponseCache(Duration = 600, VaryByQueryKeys = ["newsCategory", "newsKeyword"])]
        public async Task<IActionResult> GetDayTips(string newsCategory, string newsKeyword = "")
        {
            try
            {
                #region Validation

                var validationErrors = new List<KeyValuePair<string, string>>(); 
                if (!string.IsNullOrWhiteSpace(newsCategory) 
                    && !Enum.TryParse(newsCategory, out NewsArticleCategory newsArticleCategory))
                {
                    validationErrors.Add(new KeyValuePair<string, string>(Errors.InvalidValue, "Please assing a valid value for property newsCategory"));
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

                bool foundNews = getTipOfTodayResult?.News.Count() > 0;

                return Ok(new DayTipsResponseDto
                {
                    Status = Status.SUCCESS,
                    NewsToRead = new NewsDto 
                    {
                       ResultMessage = !foundNews ? "Could not retrieve top headlines of today, please try again." : $"Found {getTipOfTodayResult?.News.Count()} articles.",
                       TopArticleHeadlines = foundNews
                        ? getTipOfTodayResult?.News.Select(x=> new TopArticleHeadlineDto
                        {
                            Author = x.Author,
                            Date = x.Date,
                            Source = x.Source,
                            Title = x.Title,
                            Url = x.Url
                        })
                        : Enumerable.Empty<TopArticleHeadlineDto>()
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
