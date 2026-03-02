using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Playlists
{
    public class PlaylistLinksDto
    {
        [JsonPropertyName("self")]
        public string Self { get; set; } = string.Empty; 
    }
}