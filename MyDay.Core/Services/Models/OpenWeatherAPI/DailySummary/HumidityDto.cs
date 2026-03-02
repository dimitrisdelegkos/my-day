using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI.DailySummary
{
    public class PressureDto
    {
        [JsonPropertyName("afternoon")]
        public decimal Afternoon { get; set; }   
    }  
}