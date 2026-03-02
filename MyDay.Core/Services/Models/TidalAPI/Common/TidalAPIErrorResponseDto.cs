using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Common
{
    public class TidalAPIErrorResponseDto
    {
        [JsonPropertyName("errors")]
        public IEnumerable<TidalAPIErrorDto> Errors { get; set; } = Enumerable.Empty<TidalAPIErrorDto>();
    }
}