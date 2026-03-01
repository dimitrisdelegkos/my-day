using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI
{
    public class WindSpeedDto
    {
        [JsonPropertyName("speed")]
        public decimal Speed { get; set; }
        [JsonPropertyName("direction")]
        public decimal Direction { get; set; }
    }
}