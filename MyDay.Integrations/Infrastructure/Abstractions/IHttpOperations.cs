using MyDay.Integrations.Infrastructure.Models;

namespace MyDay.Integrations.Infrastructure.Abstractions
{
    public interface IHttpOperations
    {
        Task<HttpResponseModel> Get(HttpRequestModel request);

        //Task<TResponse> Post<TRequest, TResponse>(IEnumerable<KeyValuePair<string, string>> headers,
        //    string url,
        //    IEnumerable<KeyValuePair<string, string>> replacements,
        //    TRequest body,
        //    string httpClient,
        //    string system); 
    }
}