using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI.DailySummary
{
    public class TemperatureDto
    {
        [JsonPropertyName("min")]
        public decimal Min { get; set; } 
        [JsonPropertyName("max")]
        public decimal Max { get; set; } 
        [JsonPropertyName("afternoon")]
        public decimal Afternoon { get; set; }  
        [JsonPropertyName("night")]
        public decimal Night { get; set; }  
        [JsonPropertyName("evening")]
        public decimal Evening { get; set; } 
        [JsonPropertyName("morning")]
        public decimal Morning { get; set; } 
    } 
}