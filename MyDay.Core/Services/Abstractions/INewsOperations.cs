using MyDay.Core.Services.Models.NewsAPI.TopHeadlines;

namespace MyDay.Core.Services.Abstractions
{
    public interface INewsOperations
    {
        Task<TopHeadlinesResponseWrapper> GetTopHeadlines(string category, string keyword);
    }
}