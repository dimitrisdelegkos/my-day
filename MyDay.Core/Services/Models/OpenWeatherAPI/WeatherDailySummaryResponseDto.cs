using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.OpenWeatherAPI
{
    public class WeatherDailySummaryResponseDto
    {
        [JsonPropertyName("lat")]
        public decimal Latitude { get; set; }
        [JsonPropertyName("lon")]
        public decimal Longitude { get; set; }
        [JsonPropertyName("tz")]
        public string Timezone { get; set; } = string.Empty;
        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;
        [JsonPropertyName("units")]
        public string Units { get; set; } = string.Empty;
        [JsonPropertyName("temperature")]
        public TemperatureDto Temperature { get; set; } = new TemperatureDto();
        [JsonPropertyName("humidity")]
        public HumidityDto Humidity { get; set; } = new HumidityDto();
        [JsonPropertyName("pressure")]
        public PressureDto Pressure { get; set; } = new PressureDto();
        [JsonPropertyName("cloud_cover")]
        public CloudCoverDto CloudCover { get; set; } = new CloudCoverDto();
        [JsonPropertyName("precipitation")]
        public PrecipitationDto Precipitation { get; set; } = new PrecipitationDto();
        [JsonPropertyName("wind")]
        public WindDto Wind { get; set; } = new WindDto();
    }
} 