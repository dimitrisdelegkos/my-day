using Microsoft.Extensions.Logging;
using MyDay.Integrations.Helpers;
using MyDay.Integrations.Infrastructure.Abstractions;
using MyDay.Integrations.Infrastructure.Models;
using System;
using System.Text.Json;

namespace MyDay.Integrations.Infrastructure.Concrete
{
    public class HttpOperationsService : IHttpOperations
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HttpOperationsService> _logger;

        public HttpOperationsService(IHttpClientFactory clientFactory,
            ILogger<HttpOperationsService> logger)
        {
            _clientFactory = clientFactory ?? throw new ArgumentException(nameof(clientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HttpResponseModel> Get(HttpRequestModel request)
        {
            string correlationId = IntegrationHelper.GetCorrelationId();
            string targetSystem = request.TargetSystem;

            try
            {
                var httpClient = this._clientFactory.CreateClient(request.HttpClient);
                if (request.QueryParameters?.Any() ?? false)
                {
                    foreach (var requestUrlQueryParameter in request.QueryParameters)
                        request.Url = request.Url.Replace(requestUrlQueryParameter.Key, requestUrlQueryParameter.Value);
                }

                using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, request.Url))
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation("Content-Type", request.ContentType);
                    foreach (var header in request.Headers ?? Enumerable.Empty<KeyValuePair<string, string>>())
                        httpRequestMessage.Headers.Add(header.Key, header.Value);

                    _logger.LogTrace("GET request {CorrelationId} to: {TargetSystem}, URL: {RequestUrl}", correlationId, targetSystem, request.Url);

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
                        _logger.LogTrace("GET request {CorrelationId} to: {TargetSystem}, URL: {RequestUrl}, Response Details: {ResponseDetails}", correlationId, targetSystem, request.Url, JsonSerializer.Serialize(responseDetails));

                        if (httpResponse.IsSuccessStatusCode)
                        {
                            return new HttpResponseModel
                            {
                                Payload = responseContent
                            };
                        }
                        else
                        {
                            string integrationFailedError = $"Integration failed for target system {targetSystem}, correlationId: {correlationId}";
                            _logger.LogError("GET request {CorrelationId} to: {TargetSystem}, URL: {RequestUrl}, Error: {Error}", correlationId, targetSystem, request.Url, integrationFailedError);
                           
                            return new HttpResponseModel
                            {
                                HasError = true,
                                Errors = new List<KeyValuePair<string, string>>
                                {
                                    new KeyValuePair<string, string>(Errors.IntegrationFailed,integrationFailedError)
                                },
                                Payload = string.Empty
                            };
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("GET request {CorrelationId} to: {TargetSystem}, URL: {RequestUrl}, Exception: {Error}", correlationId, targetSystem, request.Url, exception.Message);
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
}