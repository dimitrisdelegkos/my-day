using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.NewsAPI.Common
{
    public class NewsArticleDto
    {
        [JsonPropertyName("source")]
        public SourceDto Source { get; set; } = new SourceDto();
        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
        [JsonPropertyName("urlToImage")]
        public string UrlToImage { get; set; } = string.Empty;
        [JsonPropertyName("publishedAt")]
        public string PublishedAt { get; set; } = string.Empty;
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
} 