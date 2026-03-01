using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI
{
    public class OpenWeatherAPIErrorResponseDto
    {
        [JsonPropertyName("cod")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("parameters")]
        public IEnumerable<string> Parameters { get; set; } = Enumerable.Empty<string>();
    }
} 