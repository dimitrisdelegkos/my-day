using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.NewsAPI.Common
{
    public class SourceDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}