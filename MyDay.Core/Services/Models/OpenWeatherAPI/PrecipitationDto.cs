using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI
{
    public class PrecipitationDto
    {
        [JsonPropertyName("total")]
        public decimal Total { get; set; }   
    }  
}