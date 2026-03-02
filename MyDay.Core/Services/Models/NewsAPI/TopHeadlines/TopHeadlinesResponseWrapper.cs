using MyDay.Core.Services.Models.NewsAPI.Common;

namespace MyDay.Core.Services.Models.NewsAPI.TopHeadlines
{
    public class TopHeadlinesResponseWrapper
    {
        public bool IsSuccess { get; set; }
        public TopHeadlinesResponseDto TopHeadlines { get; set; } = new TopHeadlinesResponseDto();
        public NewsAPIErrorResponseDto Error { get; set; } = new NewsAPIErrorResponseDto();
    }
}