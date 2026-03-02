using System.Text.Json.Serialization;

namespace MyDay.Core.Services.Models.TidalAPI.Authorization
{
    public class AuthorizationTokenResponseDto
    {
        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }  
    }
}