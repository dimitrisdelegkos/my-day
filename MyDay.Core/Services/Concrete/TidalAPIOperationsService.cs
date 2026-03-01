using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyDay.Core.Infrastructure.Abstractions;
using MyDay.Core.Infrastructure.Models;
using MyDay.Core.Services.Abstractions;
using MyDay.Core.Services.Models.TidalAPI;
using System.Text.Json;

namespace MyDay.Core.Services.Concrete
{
    public class TidalAPIOperationsService : IMusicOperations
    {
        private ILogger<TidalAPIOperationsService> _logger;
        private IConfiguration _configuration;
        private IHttpOperations _httpOperationsService;

        public TidalAPIOperationsService(ILogger<TidalAPIOperationsService> logger,
            IConfiguration configuration,
            IHttpOperations httpOperationsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpOperationsService = httpOperationsService ?? throw new ArgumentNullException(nameof(httpOperationsService));
        }

        #region Helpers

        private async Task<string> GetAccessToken()
        {
            try
            {
                var formUrlEncodedContentCollection = new List<KeyValuePair<string, string>>()
                {
                    new("grant_type", "client_credentials"),
                };
                var requestModel = new HttpRequestModel
                {
                    ContentType = "application/x-www-form-urlencoded",
                    Headers = new List<KeyValuePair<string, string>> { },
                    HttpClient = "tidal-api-auth",
                    QueryParameters = new List<KeyValuePair<string, string>> { },
                    TargetSystem = "TidalAPI",
                    Url = _configuration.GetValue<string>("TidalAPISettings:AuthEndpointUrl")
                };

                var getAccessTokenResult = await _httpOperationsService.Post<List<KeyValuePair<string, string>>>(requestModel, formUrlEncodedContentCollection);
                if (getAccessTokenResult.HasError)
                {
                    return string.Empty;
                }
                else
                {
                    var getAccessTokenResponse = JsonSerializer.Deserialize<AuthorizationTokenResponseDto>(getAccessTokenResult.Payload);
                    return getAccessTokenResponse?.AccessToken ?? string.Empty;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("An exception occurred during TidalAPI auth endpoint invocation: {Error}", exception.Message);
                return string.Empty;
            } 
        }

        #endregion
    }
}
