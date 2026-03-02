using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI.DailySummary
{
    public class WindDto
    {
        [JsonPropertyName("max")]
        public WindSpeedDto Max { get; set; } = new WindSpeedDto();
    }
}