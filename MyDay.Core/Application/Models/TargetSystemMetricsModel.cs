namespace MyDay.Core.Application.Models
{
    public class TargetSystemMetricsModel
    {
        public string SystemName { get; set; } = string.Empty;
        public IEnumerable<KeyValuePair<string, int>> AllocatedRequests { get; set; } = Enumerable.Empty<KeyValuePair<string, int>>();
    }
}
