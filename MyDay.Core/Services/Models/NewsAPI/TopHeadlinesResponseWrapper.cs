namespace MyDay.Core.Services.Models.NewsAPI
{
    public class TopHeadlinesResponseWrapper
    {
        public bool IsSuccess { get; set; }
        public TopHeadlinesResponseDto TopHeadlines { get; set; } = new TopHeadlinesResponseDto();
        public NewsAPIErrorResponseDto Error { get; set; } = new NewsAPIErrorResponseDto();
    }
}