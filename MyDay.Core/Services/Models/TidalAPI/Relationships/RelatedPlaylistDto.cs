using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Relationships
{
    public class RelatedPlaylistDto
    {
        [JsonPropertyName("id")]
        public string Id {  get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}