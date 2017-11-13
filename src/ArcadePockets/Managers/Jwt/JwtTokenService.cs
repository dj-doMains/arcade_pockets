using System.Collections.Generic;
using System.Net.Http;
using ArcadePockets.Models.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ArcadePockets.Managers.Jwt
{
    internal class JwtTokenService
    {
        HttpClient _httpClient;

        public JwtTokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public JwtSecurityToken GetToken(JwtManagerOptions options)
        {
            switch (options.GrantType)
            {
                case ArcadePocketsGrantType.ResourceOwner:
                    return GetTokenWithPassword(options);
                default:
                    return null;
            }
        }

        public JwtSecurityToken GetRefreshToken(JwtManagerOptions options, string refreshToken = "")
        {
            switch (options.GrantType)
            {
                case ArcadePocketsGrantType.ResourceOwner:
                    {
                        if (string.IsNullOrEmpty(refreshToken))
                            return GetTokenWithPassword(options);
                        else
                            return GetTokenFromRefreshToken(refreshToken, options);
                    }
                default:
                    return null;
            }
        }

        private JwtSecurityToken GetTokenWithPassword(JwtManagerOptions options)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, options.TokenIssuerUri);

            var keyValues = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", options.ClientID),
                new KeyValuePair<string, string>("client_secret", options.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", options.Username),
                new KeyValuePair<string, string>("password", options.Password)
            };


            request.Content = new FormUrlEncodedContent(keyValues);

            var bearerResult = _httpClient.SendAsync(request).Result;
            var bearerData = bearerResult.Content.ReadAsStringAsync().Result;

            JwtSecurityToken token = JsonConvert.DeserializeObject<JwtSecurityToken>(bearerData);

            return token;
        }

        private JwtSecurityToken GetTokenFromRefreshToken(string refreshToken, JwtManagerOptions options)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, options.TokenIssuerUri);

            var keyValues = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("client_id", options.ClientID),
                new KeyValuePair<string, string>("client_secret", options.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            };

            request.Content = new FormUrlEncodedContent(keyValues);

            var bearerResult = _httpClient.SendAsync(request).Result;
            var bearerData = bearerResult.Content.ReadAsStringAsync().Result;

            JwtSecurityToken token = JsonConvert.DeserializeObject<JwtSecurityToken>(bearerData);

            return token;
        }
    }
}