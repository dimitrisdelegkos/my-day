namespace MyDay.Core.Services.Models.NewsAPI.Common
{
    public class NewsAPIErrorResponseDto
    {
        public string Status { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}