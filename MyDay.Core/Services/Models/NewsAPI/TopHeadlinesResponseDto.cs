using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.NewsAPI
{
    public class TopHeadlinesResponseDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("totalResults")]
        public int TotalResults { get; set; }
        [JsonPropertyName("articles")]
        public IEnumerable<NewsArticleDto> Articles { get; set; } = Enumerable.Empty<NewsArticleDto>();
    }
}