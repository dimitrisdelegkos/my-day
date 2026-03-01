namespace MyDay.Integrations.Application.Models.NewsAPI
{
    public class TopHeadlinesResponseWrapper
    {
        public bool IsSuccess { get; set; }
        public TopHeadlinesResponseDto TopHeadlines { get; set; } = new TopHeadlinesResponseDto();
        public ErrorResponseDto Error { get; set; } = new ErrorResponseDto();
    }
}