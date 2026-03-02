using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI.DailySummary
{
    public class HumidityDto
    {
        [JsonPropertyName("afternoon")]
        public decimal Afternoon { get; set; }   
    }  
}