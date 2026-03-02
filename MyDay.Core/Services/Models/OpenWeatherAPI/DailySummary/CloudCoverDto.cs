using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI.DailySummary
{
    public class CloudCoverDto
    {
        [JsonPropertyName("afternoon")]
        public decimal Afternoon { get; set; }   
    } 
}