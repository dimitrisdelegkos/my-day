namespace MyDay.Core.Infrastructure.Models
{
    public class HttpRequestModel
    {
        public string Url { get; set; } = string.Empty;
        public IEnumerable<KeyValuePair<string, string>> Headers { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();
        public IEnumerable<KeyValuePair<string, string>> QueryParameters { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();

        public string HttpClient { get; set; } = string.Empty;
        public string ContentType {  get; set; } = string.Empty;
        public string TargetSystem { get; set; } = string.Empty;
    }
}