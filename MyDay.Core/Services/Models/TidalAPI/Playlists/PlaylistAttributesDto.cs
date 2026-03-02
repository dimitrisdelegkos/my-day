using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Playlists
{
    public class PlaylistAttributesDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("bounded")]
        public bool Bounded { get; set; }
        [JsonPropertyName("duration")]
        public string Duration { get; set; } = string.Empty;
        [JsonPropertyName("numberOfItems")]
        public int NumberOfItems { get; set; }
        [JsonPropertyName("externalLinks")]
        public IEnumerable<PlaylistExternalLinkDto> ExternalLinks { get; set; } = Enumerable.Empty<PlaylistExternalLinkDto>();
        [JsonPropertyName("createdAt")]
        public string CreatedAt { get; set; } = string.Empty;
        [JsonPropertyName("lastModifiedAt")]
        public string LastModifiedAt { get; set; } = string.Empty;
        [JsonPropertyName("accessType")]
        public string AccessType { get; set; } = string.Empty;
        [JsonPropertyName("playlistType")]
        public string PlaylistType { get; set; } = string.Empty;
        [JsonPropertyName("numberOfFollowers")]
        public int NumberOfFollowers { get; set; }
    }
}