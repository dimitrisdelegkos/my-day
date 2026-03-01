namespace MyDay.Integrations.Infrastructure.Models
{
    public class HttpResponseModel
    {
        public bool HasError { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Errors { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();   
        public string Payload { get; set; }  = string.Empty;
    }
}