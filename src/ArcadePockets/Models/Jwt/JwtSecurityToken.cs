using System;
using Newtonsoft.Json;

namespace ArcadePockets.Models.Jwt
{
    public class JwtSecurityToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        private int _expiresIn;

        [JsonProperty("expires_in")]
        public int ExpiresIn
        {
            get => _expiresIn;

            set
            {
                _expiresIn = value;
                Expiration = DateTime.Now.AddSeconds(value);
            }
        }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        public DateTime Expiration { get; private set; }
    }
}