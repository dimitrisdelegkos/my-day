using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MyDay.Core.Infrastructure.Abstractions;
using MyDay.Core.Infrastructure.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace MyDay.Core.Infrastructure.Concrete
{
    public class HttpOperationsService : IHttpOperations
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HttpOperationsService> _logger;
        private readonly IMemoryCache _memoryCache;

        public HttpOperationsService(IHttpClientFactory clientFactory,
            ILogger<HttpOperationsService> logger,
            IMemoryCache memoryCache)
        {
            _clientFactory = clientFactory ?? throw new ArgumentException(nameof(clientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<HttpResponseModel> Get(HttpRequestModel request)
            => await this.MakeHttpRequest(HttpMethod.Get, request, string.Empty);

        public async Task<HttpResponseModel> Post<TRequest>(HttpRequestModel request, TRequest requestBody)
           => await this.MakeHttpRequest(HttpMethod.Post, request, JsonSerializer.Serialize(requestBody));

        #region Helpers

        private async Task<HttpResponseModel> MakeHttpRequest(HttpMethod httpMethod,
            HttpRequestModel request,
            string requestBody)
        {
            {
                string correlationId = request.CorrelationId;
                string targetSystem = request.TargetSystem;
                string methodName = httpMethod == HttpMethod.Get ? "GET" : "POST";

                try
                {
                    var performanceTimer = new Stopwatch();
                    performanceTimer.Start();

                    var httpClient = this._clientFactory.CreateClient(request.HttpClient);
                    if (request.QueryParameters?.Any() ?? false)
                    {
                        foreach (var requestUrlQueryParameter in request.QueryParameters)
                            request.Url = request.Url.Replace(requestUrlQueryParameter.Key, requestUrlQueryParameter.Value);
                    }

                    using (var httpRequestMessage = new HttpRequestMessage(httpMethod, request.Url))
                    {
                        httpRequestMessage.Headers.TryAddWithoutValidation("Content-Type", request.ContentType);
                        foreach (var header in request.Headers ?? Enumerable.Empty<KeyValuePair<string, string>>())
                            httpRequestMessage.Headers.Add(header.Key, header.Value);

                        if (!string.IsNullOrEmpty(requestBody))
                        {
                            if (request.ContentType == "application/x-www-form-urlencoded")
                            {
                                httpRequestMessage.Content = new FormUrlEncodedContent(JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(requestBody));
                            }
                            else
                            {
                                httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                            }
                        }
                        _logger.LogTrace("{HttpMethod} request {CorrelationId} to: {TargetSystem}, URL: {RequestUrl}, Body {RequestBody}", methodName, correlationId, targetSystem, request.Url, requestBody ?? "N/A");

                        using (var httpResponse = await httpClient.SendAsync(httpRequestMessage))
                        {
                            var responseContent = await httpResponse.Content.ReadAsStringAsync();
                            var responseDetails = new Dictionary<string, object>
                            {
                                ["CorrelationId"] = correlationId,
                                ["Headers"] = httpResponse.Content.Headers.ToDictionary(x => x.Key, x => x.Value),
                                ["Paylod"] = responseContent,
                                ["StatusCode"] = httpResponse.StatusCode,
                            };
                            _logger.LogTrace("{HttpMethod} request {CorrelationId} to: {TargetSystem}, URL: {RequestUrl}, Response Details: {ResponseDetails}", methodName, correlationId, targetSystem, request.Url, JsonSerializer.Serialize(responseDetails));

                            string payload = string.Empty;
                            bool hasError = false;
                            var errors = new List<KeyValuePair<string, string>>();

                            if (httpResponse.IsSuccessStatusCode)
                            {
                                payload = responseContent;
                            }
                            else
                            {
                                string integrationFailedError = $"Integration failed for target system {targetSystem}, correlationId: {correlationId}";
                                _logger.LogError("{HttpMethod} request {CorrelationId} to: {TargetSystem}, URL: {RequestUrl}, Error: {Error}", httpMethod, correlationId, targetSystem, request.Url, integrationFailedError);

                                hasError = true;
                                errors = new List<KeyValuePair<string, string>>
                                    {
                                        new KeyValuePair<string, string>(Errors.IntegrationFailed,integrationFailedError)
                                    };
                            }

                            #region Performance Metrics

                            performanceTimer.Stop();
                            var performanceMetric = (targetSystem, correlationId, performanceTimer.Elapsed.TotalMilliseconds);

                            var externalAPICallsMetrics = _memoryCache.Get<List<(string TargetSystem, string CorrelationId, double TotalMilliseconds)>>("external-api-calls-metrics");
                            if (externalAPICallsMetrics == null)
                            {
                                _memoryCache.Set("external-api-calls-metrics", new List<(string, string, double)> { performanceMetric });
                            }
                            else 
                            {
                                externalAPICallsMetrics.Add(performanceMetric);
                                _memoryCache.Set("external-api-calls-metrics", externalAPICallsMetrics);
                            }

                            #endregion

                            return new HttpResponseModel
                            {
                                HasError = hasError,
                                Errors = errors,
                                Payload = payload
                            };
                        }
                    } 
                }
                catch (Exception exception)
                {
                    _logger.LogError("{HttpMethod} request {CorrelationId} to: {TargetSystem}, URL: {RequestUrl}, Exception: {Error}", httpMethod, correlationId, targetSystem, request.Url, exception.Message);
                    return new HttpResponseModel
                    {
                        HasError = true,
                        Errors = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>(Errors.Generic,exception.Message)
                        },
                        Payload = string.Empty
                    };
                }
            }
        }

        #endregion
    }
}