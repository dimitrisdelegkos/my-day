namespace MyDay.Core.Application.Models
{
    public class DayTipsModel
    {
        public IEnumerable<ArticleModel>? News { get; set; } = Enumerable.Empty<ArticleModel>();
        public WeatherSummaryModel? WeatherSummary { get; set; } = new WeatherSummaryModel();
    }
}
