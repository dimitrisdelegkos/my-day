using MyDay.Core.Application.Models;

namespace MyDay.Core.Application.Abstractions
{
    public interface IDailyTipsOperations
    {
        Task<DayTipsModel?> GetTipsOfToday(NewsFilteringCriteriaModel newsFilteringCriteria, 
            WeatherFilteringCriteriaModel weatherFilteringCriteria);
    }
}
