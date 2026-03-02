namespace MyDay.API.Models
{
    public class TargetSystemMetricsDto
    {
        /// <summary>
        /// The name of the system
        /// </summary>
        public string SystemName { get; set; } = string.Empty;
        /// <summary>
        /// The count of the requests of the system allocated as fast, average or slow 
        /// </summary>
        public Dictionary<string, int> AllocatedRequests { get; set; } = new Dictionary<string, int>();
    }
}
