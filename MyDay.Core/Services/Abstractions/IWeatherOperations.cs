using MyDay.Core.Services.Models.OpenWeatherAPI.DailySummary;

namespace MyDay.Core.Services.Abstractions
{
    public interface IWeatherOperations
    {
        Task<WeatherDailySummaryResponseWrapper> GetWeatherDailySummary(decimal latitude, decimal longitude, string date);
    }
}
