using MyDay.Core.Application.Models;

namespace MyDay.Core.Application.Abstractions
{
    public interface IPerformanceOperations
    {
        Task<IEnumerable<TargetSystemMetricsModel>> GetPerformanceMetrics();
    }
}
