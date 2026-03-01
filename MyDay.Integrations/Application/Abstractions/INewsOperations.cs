using MyDay.Integrations.Application.Models.NewsAPI;

namespace MyDay.Integrations.Application.Abstractions
{
    public interface INewsOperations
    {
        Task<TopHeadlinesResponseWrapper> GetTopHeadlines(string category, string keyword);
    }
}