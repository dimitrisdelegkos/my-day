namespace MyDay.Integrations.Application.Models.NewsAPI
{
    public class TopHeadlinesResponseDto
    {
        public string Status { get; set; } = string.Empty;
        public int TotalResults { get; set; }
        public IEnumerable<NewsArticleDto> Articles { get; set; } = Enumerable.Empty<NewsArticleDto>();
    }
}