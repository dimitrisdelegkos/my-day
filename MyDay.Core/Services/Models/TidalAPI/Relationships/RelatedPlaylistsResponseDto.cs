using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Relationships
{
    public class RelatedPlaylistsResponseDto
    {
        [JsonPropertyName("data")]
        public IEnumerable<RelatedPlaylistDto> Data { get; set; } = Enumerable.Empty<RelatedPlaylistDto>();
        [JsonPropertyName("links")]
        public RelatedPlaylistsLinksDto Links { get; set; } = new RelatedPlaylistsLinksDto();
    }
}