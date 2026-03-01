using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI
{
    public class MusicPlaylistDto
    {
        [JsonPropertyName("id")]
        public string Id {  get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}