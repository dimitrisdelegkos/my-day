using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI.DailySummary
{
    public class PrecipitationDto
    {
        [JsonPropertyName("total")]
        public decimal Total { get; set; }   
    }  
}