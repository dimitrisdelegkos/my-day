using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MyDay.Core.Application.Abstractions;
using MyDay.Core.Application.Models;

namespace MyDay.Core.Application.Concrete
{
    public class PerformanceOperationsService : IPerformanceOperations
    {
        private readonly ILogger<PerformanceOperationsService> _logger;
        private readonly IMemoryCache _memoryCache;

        public PerformanceOperationsService(ILogger<PerformanceOperationsService> logger,
            IMemoryCache memoryCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<IEnumerable<TargetSystemMetricsModel>> GetPerformanceMetrics()
        {
            {
                try
                {
                    var externalAPICallsMetrics = _memoryCache.Get<List<(string TargetSystem, string CorrelationId, double TotalMilliseconds)>>("external-api-calls-metrics");
                    if (externalAPICallsMetrics == null) 
                    {
                        return Enumerable.Empty<TargetSystemMetricsModel>();
                    }

                    var performanceMetrics = new List<TargetSystemMetricsModel>();
                    foreach (var targetSystem in externalAPICallsMetrics.GroupBy(x => x.TargetSystem))
                    {
                        var targetSystemMetric = new TargetSystemMetricsModel
                        {
                            SystemName = targetSystem.Key,
                            AllocatedRequests = new List<KeyValuePair<string, int>>
                            {
                                new KeyValuePair<string, int>("fast", targetSystem.Where(x=>x.TotalMilliseconds <=200 ).Count()),
                                new KeyValuePair<string, int>("average", targetSystem.Where(x=>x.TotalMilliseconds >200 && x.TotalMilliseconds <400 ).Count()),
                                new KeyValuePair<string, int>("slow", targetSystem.Where(x=>x.TotalMilliseconds >=400 ).Count()),
                            }
                        };
                        performanceMetrics.Add(targetSystemMetric);
                    }

                    return await Task.FromResult(performanceMetrics);
                }
                catch (Exception exception)
                {
                    _logger.LogError("An exception occurred during metrics: {Error}", exception.Message);
                    return Enumerable.Empty<TargetSystemMetricsModel>();
                }
            }
        }
    }
}
