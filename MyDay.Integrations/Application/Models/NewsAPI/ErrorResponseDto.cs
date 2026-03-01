namespace MyDay.Integrations.Application.Models.NewsAPI
{
    public class ErrorResponseDto
    {
        public string Status { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}