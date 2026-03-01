namespace MyDay.API.Models
{
    public class WeatherDailySummaryDto
    {
        /// <summary>
        /// The max temperature for today in Celsius
        /// </summary>
        public decimal MaximumTemperature { get; set; }
        /// <summary>
        /// The min temperature for today in Celsius
        /// </summary>
        public decimal MinimumTemperature { get; set; }
        /// <summary>
        /// The max speed wind for today 
        /// </summary>
        public decimal MaxWindSpeed { get; set; }
        /// <summary>
        /// The value of the humidity 
        /// </summary>
        public decimal Humidity { get; set; }
    }
}
