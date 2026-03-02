namespace MyDay.Core.Application.Models.Weather
{
    public class WeatherSummaryModel
    {
        public decimal MaximumTemperature { get; set; }
        public decimal MinimumTemperature { get; set; }
        public decimal MaxWindSpeed { get; set; }
        public decimal Humidity { get; set; }
    }
}