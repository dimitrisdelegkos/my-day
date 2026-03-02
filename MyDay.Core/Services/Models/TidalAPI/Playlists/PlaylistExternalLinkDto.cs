using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Playlists
{
    public class PlaylistExternalLinkDto
    {
        [JsonPropertyName("href")]
        public string Href { get; set; } = string.Empty;
        [JsonPropertyName("meta")]
        public Dictionary<string,string> Meta { get; set; } = new Dictionary<string, string>();
    }
} 