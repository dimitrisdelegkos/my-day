using MyDay.API.Enums;
using System.Text.Json.Serialization;

namespace MyDay.API.Models
{
    public class PerformanceMetricsResponseDto
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
        /// The metrics of performance, presented per integrated system
        /// </summary>
        public IEnumerable<TargetSystemMetricsDto> Metrics { get; set; } = Enumerable.Empty<TargetSystemMetricsDto>(); 
    }
}