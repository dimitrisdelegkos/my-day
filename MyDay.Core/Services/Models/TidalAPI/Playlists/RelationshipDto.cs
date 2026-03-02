using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Playlists
{
    public class RelationshipDto
    {
        [JsonPropertyName("links")]
        public PlaylistLinksDto Links { get; set; } = new PlaylistLinksDto();
    }
}
