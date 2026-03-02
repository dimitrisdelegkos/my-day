using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Common
{
    public class TidalAPIErrorDto
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
        [JsonPropertyName("detail")]
        public string Detail { get; set; } = string.Empty;
        [JsonPropertyName("meta")]
        public Dictionary<string, string> Meta { get; set; } = new Dictionary<string, string>();
    }
}