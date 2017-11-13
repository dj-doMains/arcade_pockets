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
                case ArcadePocketsGrantType.Password:
                    return GetTokenWithPasswordGrantType(options);
                default:
                    return null;
            }
        }

        public JwtSecurityToken GetRefreshToken(string refreshToken, JwtManagerOptions options)
        {
            switch (options.GrantType)
            {
                case ArcadePocketsGrantType.Password:
                    return GetRefreshTokenWithPasswordGrantType(refreshToken, options);
                default:
                    return null;
            }
        }

        private JwtSecurityToken GetTokenWithPasswordGrantType(JwtManagerOptions options)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, options.TokenIssuerUri);

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("client_id", options.ClientID));
            keyValues.Add(new KeyValuePair<string, string>("client_secret", options.ClientSecret));
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "password"));
            keyValues.Add(new KeyValuePair<string, string>("username", options.Username));
            keyValues.Add(new KeyValuePair<string, string>("password", options.Password));

            request.Content = new FormUrlEncodedContent(keyValues);

            var bearerResult = _httpClient.SendAsync(request).Result;
            var bearerData = bearerResult.Content.ReadAsStringAsync().Result;

            JwtSecurityToken token = JsonConvert.DeserializeObject<JwtSecurityToken>(bearerData);

            return token;
        }

        private JwtSecurityToken GetRefreshTokenWithPasswordGrantType(string refreshToken, JwtManagerOptions options)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, options.TokenIssuerUri);

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("client_id", options.ClientID));
            keyValues.Add(new KeyValuePair<string, string>("client_secret", options.ClientSecret));
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            keyValues.Add(new KeyValuePair<string, string>("refresh_token", refreshToken));

            request.Content = new FormUrlEncodedContent(keyValues);

            var bearerResult = _httpClient.SendAsync(request).Result;
            var bearerData = bearerResult.Content.ReadAsStringAsync().Result;

            JwtSecurityToken token = JsonConvert.DeserializeObject<JwtSecurityToken>(bearerData);

            return token;
        }
    }
}