namespace MyDay.Core.Services.Models.OpenWeatherAPI
{
    public class WeatherDailySummaryResponseWrapper
    {
        public bool IsSuccess { get; set; }
        public WeatherDailySummaryResponseDto WeatherDailySummary { get; set; } = new WeatherDailySummaryResponseDto();
        public OpenWeatherAPIErrorResponseDto Error { get; set; } = new OpenWeatherAPIErrorResponseDto();
    }
}