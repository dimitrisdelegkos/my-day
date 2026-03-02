using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Playlists
{
    public class PlaylistResponseDto
    {
        [JsonPropertyName("data")]
        public PlaylistDto Data { get; set; } = new PlaylistDto();
        [JsonPropertyName("links")]
        public PlaylistLinksDto Links { get; set; } = new PlaylistLinksDto();
    }
}