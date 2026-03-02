using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyDay.Core.Application.Abstractions;
using MyDay.Core.Application.Models;
using MyDay.Core.Application.Models.Music;
using MyDay.Core.Application.Models.News;
using MyDay.Core.Application.Models.Weather;
using MyDay.Core.Infrastructure.Abstractions;
using MyDay.Core.Services.Abstractions;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace MyDay.Core.Application.Concrete
{
    public class DailyTipsOperationsService : IDailyTipsOperations
    {
        private ILogger<DailyTipsOperationsService> _logger;
        private IConfiguration _configuration;
        private ICachingOperations _cachingOperationsService;

        private INewsOperations _newsOperationsService; 
        private IWeatherOperations _weatherOperationsService;
        private IMusicOperations _musicOperationsService;

        public DailyTipsOperationsService(ILogger<DailyTipsOperationsService> logger,
            IConfiguration configuration,
            ICachingOperations cachingOperationsService,
            INewsOperations newsOperationsService,
            IWeatherOperations weatherOperationsService,
            IMusicOperations musicOperationsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cachingOperationsService = cachingOperationsService ?? throw new ArgumentNullException(nameof(cachingOperationsService));
            _newsOperationsService = newsOperationsService ?? throw new ArgumentNullException(nameof(newsOperationsService));
            _weatherOperationsService = weatherOperationsService ?? throw new ArgumentNullException(nameof(weatherOperationsService));
            _musicOperationsService = musicOperationsService ?? throw new ArgumentNullException(nameof(musicOperationsService)); ;
        }

        public async Task<DayTipsModel?> GetTipsOfToday(NewsFilteringCriteriaModel newsFilteringCriteria, 
            WeatherFilteringCriteriaModel weatherFilteringCriteria,
            MusicFilteringCriteriaModel musicFilteringCriteria)
        {
            try
            {
                string filterConcatenated = $"newsCategory:{newsFilteringCriteria.Category}|" +
                    $"newsKeyword:{newsFilteringCriteria.Keyword}|" +
                    $"weatherLat:{weatherFilteringCriteria.Latitude.ToString()}|" +
                    $"weatherLon:{weatherFilteringCriteria.Longitude.ToString()}|" +
                    $"musicKeyword:{musicFilteringCriteria.Keyword}";
                string filterConcatenatedEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(filterConcatenated));
                string cacheKey = $"dailytipquery:{filterConcatenatedEncoded}";

                var (isCached, cacheValue) = await _cachingOperationsService.GetSetCacheEntry(cacheKey, async () =>
                {
                    int topHeadlinesCount = _configuration.GetValue<int>("DailyTipsSettings:TopHeadlinesCount");
                    int playlistsCount = _configuration.GetValue<int>("DailyTipsSettings:PlaylistsCount");

                    var getTopNewsTask = this.GetNewsTopHeadlines(newsFilteringCriteria, topHeadlinesCount);
                    var getWeatherSummaryTask = this.GetWeatherSummary(weatherFilteringCriteria);
                    var getMusicPlaylistsTask = this.GetMusicPlaylists(musicFilteringCriteria, playlistsCount);
                    await Task.WhenAll(getTopNewsTask, getWeatherSummaryTask, getMusicPlaylistsTask);

                    return new DayTipsModel
                    {
                        News = getTopNewsTask.Result,
                        WeatherSummary = getWeatherSummaryTask.Result,
                        Playlists = getMusicPlaylistsTask.Result
                    };
                });

                if (isCached)
                    return cacheValue;

                return null;
            }
            catch (Exception exception) 
            {
                _logger.LogError("An exception occurred during fetching daily tips: {Error}", exception.Message);
                return null;

            }
        }

        #region Helpers

        private async Task<IEnumerable<ArticleModel>?> GetNewsTopHeadlines(NewsFilteringCriteriaModel newsFilteringCriteria, int topHeadlinesCount)
        {
            var dailyTopNewsHeadlinesResult = await _newsOperationsService.GetTopHeadlines(newsFilteringCriteria.Category, newsFilteringCriteria.Keyword);
            if (!dailyTopNewsHeadlinesResult.IsSuccess
                || dailyTopNewsHeadlinesResult.TopHeadlines.TotalResults <= 0)
            {
                _logger.LogTrace("No news could be retrieved for criteria: {NewsCriteria}", JsonSerializer.Serialize(newsFilteringCriteria));
                return null;
            }
            else
            {
                return dailyTopNewsHeadlinesResult.TopHeadlines.Articles.Take(topHeadlinesCount).Select(x => new ArticleModel
                {
                    Author = x.Author,
                    Date = DateTime.Parse(x.PublishedAt, null, DateTimeStyles.RoundtripKind).ToString("dd/MM/yyyy"),
                    Source = x.Source.Name,
                    Title = x.Title,
                    Url = x.Url
                });
            }
        }

        private async Task<WeatherSummaryModel?> GetWeatherSummary(WeatherFilteringCriteriaModel weatherFilteringCriteria)
        {
            decimal latitude = weatherFilteringCriteria.Latitude == null
                    ? _configuration.GetValue<decimal>("DailyTipsSettings:LocationLatitude")
                    : weatherFilteringCriteria.Latitude.Value;
            decimal longitude = weatherFilteringCriteria.Longitude == null
                ? _configuration.GetValue<decimal>("DailyTipsSettings:LocationLongitude")
                : weatherFilteringCriteria.Longitude.Value;

            var dailyWeatherSummaryResult = await _weatherOperationsService.GetWeatherDailySummary(latitude, longitude, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!dailyWeatherSummaryResult.IsSuccess)
            {
                _logger.LogTrace("No weather data could be retrieved for criteria: {WeatherCriteria}", JsonSerializer.Serialize(weatherFilteringCriteria));
                return null;
            }
            else
            {
                return new WeatherSummaryModel
                {
                    MaximumTemperature = dailyWeatherSummaryResult.WeatherDailySummary.Temperature.Max,
                    MinimumTemperature = dailyWeatherSummaryResult.WeatherDailySummary.Temperature.Min,
                    Humidity = dailyWeatherSummaryResult.WeatherDailySummary.Humidity.Afternoon,
                    MaxWindSpeed = dailyWeatherSummaryResult.WeatherDailySummary.Wind.Max.Speed
                };
            }
        }

        private async Task<IEnumerable<PlaylistModel>?> GetMusicPlaylists (MusicFilteringCriteriaModel musicFilteringCriteria, int playlistsCount)
        {
            var relatedPlaylistsResult = await _musicOperationsService.GetRelatedPlaylists(musicFilteringCriteria.Keyword);
            if (!relatedPlaylistsResult.IsSuccess)
            {
                _logger.LogTrace("No playlist data could be retrieved for criteria: {MusicCriteria}", JsonSerializer.Serialize(musicFilteringCriteria));
                return null;
            }

            var relatedPlaylistsIds = relatedPlaylistsResult.RelatedPlaylists.Data.Take(playlistsCount).Select(x => x.Id);
            if(!relatedPlaylistsIds.Any())
            {
                _logger.LogTrace("No playlist IDs could be retrieved for criteria: {MusicCriteria}", JsonSerializer.Serialize(musicFilteringCriteria));
                return null;
            }

            var getPlaylistsTasks = relatedPlaylistsIds.Select(x => _musicOperationsService.GetPlaylist(x));
            var getPlaylistsResult = await Task.WhenAll(getPlaylistsTasks);
            if (getPlaylistsResult.All(x => !x.IsSuccess))
            {
                _logger.LogTrace("No playlist data could be retrieved for playlists: {PlaylistIds}", JsonSerializer.Serialize(relatedPlaylistsIds));
                return null;
            }

            var playlists = getPlaylistsResult.Select(x =>
            {
                if (!x.IsSuccess)
                {
                    _logger.LogTrace("No playlist data could be retrieved for playlist: {Error}", x.Error);
                    return null;
                }

                var playlistData = x.Playlist.Data;
                var playlistlinks = playlistData.Attributes.ExternalLinks;

                return new PlaylistModel
                {
                    Name = playlistData.Attributes.Name,
                    Description = playlistData.Attributes.Description,
                    Songs = playlistData.Attributes.NumberOfItems,
                    Link = playlistlinks.Any(y => y.Meta.ContainsKey("type") && y.Meta["type"] == "TIDAL_AUTOPLAY_WEB")
                        ? playlistlinks.First(y => y.Meta.ContainsKey("type") && y.Meta["type"] == "TIDAL_AUTOPLAY_WEB").Href
                        : playlistlinks.First().Href
                };
            }).Where(x => x != null);

            return playlists;
        }

        #endregion
    }
}
