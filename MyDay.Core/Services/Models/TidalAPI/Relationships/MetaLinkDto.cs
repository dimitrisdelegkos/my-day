using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Relationships
{
    public class MetaLinkDto
    {
        [JsonPropertyName("nextCursor")]
        public string NextCursor {  get; set; } = string.Empty;
    }
}