using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Relationships
{
    public class RelatedPlaylistsLinksDto
    {
        [JsonPropertyName("self")]
        public string Self { get; set; } = string.Empty;
        [JsonPropertyName("next")]
        public string Next { get; set; } = string.Empty;
        [JsonPropertyName("meta")]
        public MetaLinkDto Meta { get; set; } = new MetaLinkDto();
    }
}