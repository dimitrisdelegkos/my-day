using Microsoft.AspNetCore.Mvc;
using MyDay.API.Enums;
using MyDay.API.Models;
using MyDay.Core;
using MyDay.Core.Application.Abstractions;

namespace MyDay.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PerformanceController : ControllerBase
    {

        private readonly ILogger<PerformanceController> _logger;
        private readonly IPerformanceOperations _performanceOperationsService;

        public PerformanceController(ILogger<PerformanceController> logger,
            IDailyTipsOperations dailyTipsOperationsService,
            IPerformanceOperations performanceOperationsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _performanceOperationsService = performanceOperationsService ?? throw new ArgumentNullException(nameof(performanceOperationsService)); ;
        }
         
        /// <summary>
        /// Returns the performance metrics per integrated API
        /// </summary>
        /// <returns><see cref="PerformanceMetricsResponseDto"/></returns>
        /// <response code="200">The request was processed successfully</response>
        /// <response code="404">If no metrics are found, 404 is returned</response>
        /// <response code="500">Internal Server Error. Response should describe the cause of failure</response>
        [ProducesResponseType(typeof(PerformanceMetricsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PerformanceMetricsResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PerformanceMetricsResponseDto), StatusCodes.Status500InternalServerError)]
        [HttpGet(Name = "GetPerformanceMetrics")]
        public async Task<IActionResult> GetPerformanceMetrics()
        {
            try
            {
                var getPerformanceMetricsResult = await _performanceOperationsService.GetPerformanceMetrics();
                if (!getPerformanceMetricsResult.Any())
                {
                    return StatusCode(
                       StatusCodes.Status404NotFound,
                       new DayTipsResponseDto
                       {
                           Status = Status.FAILURE,
                           Errors = new Dictionary<string, string>() { [Errors.Generic] = "No metrics were found" }
                       });
                }
                 
                return Ok(new PerformanceMetricsResponseDto
                {
                    Status = Status.SUCCESS,
                    Metrics = getPerformanceMetricsResult.Select(x=> new TargetSystemMetricsDto
                    {
                        SystemName = x.SystemName,
                        AllocatedRequests = x.AllocatedRequests.ToDictionary(x => x.Key, x => x.Value)
                    })
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return StatusCode(
                       StatusCodes.Status500InternalServerError,
                       new PerformanceMetricsResponseDto
                       {
                           Status = Status.FAILURE,
                           Errors = new Dictionary<string, string>() { [Errors.Generic] = exception.Message }
                       });
            }
        }
    }
}
