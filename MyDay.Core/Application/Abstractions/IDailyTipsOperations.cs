using MyDay.Core.Application.Models;
using MyDay.Core.Application.Models.Music;
using MyDay.Core.Application.Models.News;
using MyDay.Core.Application.Models.Weather;

namespace MyDay.Core.Application.Abstractions
{
    public interface IDailyTipsOperations
    {
        Task<DayTipsModel?> GetTipsOfToday(NewsFilteringCriteriaModel newsFilteringCriteria, 
            WeatherFilteringCriteriaModel weatherFilteringCriteria,
            MusicFilteringCriteriaModel musicFilteringCriteria);
    }
}
