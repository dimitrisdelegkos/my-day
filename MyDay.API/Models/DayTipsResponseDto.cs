using MyDay.API.Enums;
using System.Text.Json.Serialization;

namespace MyDay.API.Models
{
    public class DayTipsResponseDto
    {
        /// <summary>
        /// Indicates the result of a processed request. 
        /// </summary> 
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }
        /// <summary>
        /// In case of a failed request, all the errors are returned to this array, in the form of error code-error message pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Errors { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();
        /// <summary>
        /// Contains all the proposed articles that somebody can read today
        /// </summary>
        public NewsDto NewsToRead { get; set; } = new NewsDto();
        /// <summary>
        /// The details of the weather for today (i.e. temperature, humidity, wind speed)
        /// </summary>
        public WeatherDailyReportDto WeatherPrognosis { get; set; } = new WeatherDailyReportDto();
        /// <summary>
        /// A list of proposed music to listen to today
        /// </summary>
        public MusicDto MusicForToday { get; set; } = new MusicDto();
    }
}