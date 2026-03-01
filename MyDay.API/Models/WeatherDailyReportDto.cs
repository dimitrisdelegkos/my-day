namespace MyDay.API.Models
{
    public class WeatherDailyReportDto
    {
        /// <summary>
        /// A message indicating the result of the operation
        /// </summary>
        public string ResultMessage { get; set; } = string.Empty; 
        /// <summary>
        /// The weather report for today
        /// </summary>
        public WeatherDailySummaryDto WeatherDailySummary { get; set; } = new WeatherDailySummaryDto();
    }
}
