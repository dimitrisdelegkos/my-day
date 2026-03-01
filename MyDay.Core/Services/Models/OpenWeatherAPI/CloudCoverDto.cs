using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI
{
    public class CloudCoverDto
    {
        [JsonPropertyName("afternoon")]
        public decimal Afternoon { get; set; }   
    } 
}