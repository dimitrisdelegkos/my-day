using MyDay.Core.Services.Models.OpenWeatherAPI.Common;

namespace MyDay.Core.Services.Models.OpenWeatherAPI.DailySummary
{
    public class WeatherDailySummaryResponseWrapper
    {
        public bool IsSuccess { get; set; }
        public WeatherDailySummaryResponseDto WeatherDailySummary { get; set; } = new WeatherDailySummaryResponseDto();
        public OpenWeatherAPIErrorResponseDto Error { get; set; } = new OpenWeatherAPIErrorResponseDto();
    }
}