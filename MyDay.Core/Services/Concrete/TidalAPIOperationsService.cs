using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyDay.Core.Infrastructure.Abstractions;
using MyDay.Core.Infrastructure.Models;
using MyDay.Core.Services.Abstractions;
using MyDay.Core.Services.Models.TidalAPI.Authorization;
using MyDay.Core.Services.Models.TidalAPI.Common;
using MyDay.Core.Services.Models.TidalAPI.Playlists;
using MyDay.Core.Services.Models.TidalAPI.Relationships;
using System.Text;
using System.Text.Json;
using System.Web;

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

        public async Task<RelatedPlaylistsResponseWrapper> GetRelatedPlaylists(string keyword)
        {
            try
            {
                var accessToken = await this.GetAccessToken();
                if (String.IsNullOrWhiteSpace(accessToken))
                {
                    _logger.LogError("An error occurred during TidalAPI-GetRelatedPlaylists invocation: {Error}", "Could not retrieve access token");
                    return new RelatedPlaylistsResponseWrapper
                    {
                        IsSuccess = false
                    };
                }

                string baseEndpointUrl = _configuration.GetValue<string>("TidalAPISettings:EndpointUrl");
                string requestUrl = "/searchResults/{keyword}/relationships/playlists";
                var requestQueryParameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("{keyword}", HttpUtility.UrlEncode(keyword)),
                };

                var requestModel = new HttpRequestModel
                {
                    ContentType = "application/json",
                    Headers = new List<KeyValuePair<string, string>> 
                    {
                        new KeyValuePair<string, string>("Authorization", $"Bearer {accessToken}"),
                        new KeyValuePair<string, string>("accept","application/vnd.api+json")
                    },
                    HttpClient = "tidal-api",
                    QueryParameters = requestQueryParameters,
                    TargetSystem = "TidalAPI",
                    Url = baseEndpointUrl + requestUrl
                };
                var getRelatedPlaylistsResult = await _httpOperationsService.Get(requestModel);

                if (getRelatedPlaylistsResult.HasError)
                {
                    return new RelatedPlaylistsResponseWrapper
                    {
                        IsSuccess = false,
                        Error = JsonSerializer.Deserialize<TidalAPIErrorResponseDto>(getRelatedPlaylistsResult.Payload)
                    };
                }
                else
                {
                    return new RelatedPlaylistsResponseWrapper
                    {
                        IsSuccess = true,
                        RelatedPlaylists = JsonSerializer.Deserialize<RelatedPlaylistsResponseDto>(getRelatedPlaylistsResult.Payload)
                    };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("An exception occurred during TidalAPI-GetRelatedPlaylists invocation: {Error}", exception.Message);
                return new RelatedPlaylistsResponseWrapper
                {
                    IsSuccess = false
                };
            }
        }

        public async Task<PlaylistResponseWrapper> GetPlaylist(string playlistId)
        {
            try
            {
                var accessToken = await this.GetAccessToken();
                if (String.IsNullOrWhiteSpace(accessToken))
                {
                    _logger.LogError("An error occurred during TidalAPI-GetPlaylist invocation: {Error}", "Could not retrieve access token");
                    return new PlaylistResponseWrapper
                    {
                        IsSuccess = false
                    };
                }

                string baseEndpointUrl = _configuration.GetValue<string>("TidalAPISettings:EndpointUrl");
                string requestUrl = "/playlists/{playlistId}";
                var requestQueryParameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("{playlistId}", HttpUtility.UrlEncode(playlistId)),
                };

                var requestModel = new HttpRequestModel
                {
                    ContentType = "application/json",
                    Headers = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Authorization", $"Bearer {accessToken}"),
                        new KeyValuePair<string, string>("accept","application/vnd.api+json")
                    },
                    HttpClient = "tidal-api",
                    QueryParameters = requestQueryParameters,
                    TargetSystem = "TidalAPI",
                    Url = baseEndpointUrl + requestUrl
                };
                var getPlaylistResult = await _httpOperationsService.Get(requestModel);

                if (getPlaylistResult.HasError)
                {
                    return new PlaylistResponseWrapper
                    {
                        IsSuccess = false,
                        Error = JsonSerializer.Deserialize<TidalAPIErrorResponseDto>(getPlaylistResult.Payload)
                    };
                }
                else
                {
                    return new PlaylistResponseWrapper
                    {
                        IsSuccess = true,
                        Playlist = JsonSerializer.Deserialize<PlaylistResponseDto>(getPlaylistResult.Payload)
                    };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("An exception occurred during TidalAPI-GetPlaylist invocation: {Error}", exception.Message);
                return new PlaylistResponseWrapper
                {
                    IsSuccess = false
                };
            }
        }


        #region Helpers

        private async Task<string> GetAccessToken()
        {
            try
            {
                string clientCredentials = $"{_configuration.GetValue<string>("TidalAPISettings:ClientId")}:{_configuration.GetValue<string>("TidalAPISettings:ClientSecret")}";
                string basicAuthHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientCredentials));

                var formUrlEncodedContentCollection = new List<KeyValuePair<string, string>>()
                {
                    new("grant_type", "client_credentials"),
                };
                var requestModel = new HttpRequestModel
                {
                    ContentType = "application/x-www-form-urlencoded",
                    Headers = new List<KeyValuePair<string, string>> 
                    {
                        new KeyValuePair<string, string>("Authorization",$"Basic {basicAuthHeaderValue}")
                    },
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
