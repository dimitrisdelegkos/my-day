using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Playlists
{
    public class PlaylistDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("attributes")]
        public PlaylistAttributesDto Attributes { get; set; } = new PlaylistAttributesDto();
        [JsonPropertyName("relationships")]
        public Dictionary<string, RelationshipDto> Relationships { get; set; } = new Dictionary<string, RelationshipDto>();
    }
}

