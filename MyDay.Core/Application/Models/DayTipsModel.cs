using MyDay.Core.Application.Models.Music;
using MyDay.Core.Application.Models.News;
using MyDay.Core.Application.Models.Weather;

namespace MyDay.Core.Application.Models
{
    public class DayTipsModel
    {
        public IEnumerable<ArticleModel>? News { get; set; } = Enumerable.Empty<ArticleModel>();
        public WeatherSummaryModel? WeatherSummary { get; set; } = new WeatherSummaryModel();
        public IEnumerable<PlaylistModel>? Playlists { get; set; } = Enumerable.Empty<PlaylistModel>();
    }
}