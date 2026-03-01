using MyDay.Core.Infrastructure.Models;

namespace MyDay.Core.Infrastructure.Abstractions
{
    public interface IHttpOperations
    {
        Task<HttpResponseModel> Get(HttpRequestModel request);

        Task<HttpResponseModel> Post<TRequest>(HttpRequestModel request, TRequest requestBody);
    }
}