using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using ArcadePockets.Managers.Jwt;
using ArcadePockets.Models.Jwt;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArcadePockets.Managers.Jwt
{
    public class JwtManager : IJwtManager
    {
        IMemoryCache _cache;
        JwtManagerOptions _options;

        static HttpClient _httpClient;

        public JwtManager(IMemoryCache cache, IOptions<JwtManagerOptions> options)
        {
            _cache = cache;
            _options = options.Value;
        }

        static JwtManager()
        {
            _httpClient = new HttpClient();
        }

        public string AccessToken
        {
            get
            {
                JwtSecurityToken jwtSecurityToken = null;

                if (!_cache.TryGetValue(_options.CacheKey, out jwtSecurityToken))
                {
                    jwtSecurityToken = GetToken();

                    if (jwtSecurityToken != null)
                        SetCache(jwtSecurityToken);
                }

                if (jwtSecurityToken != null)
                    if (DateTime.Now > jwtSecurityToken.Expiration.AddSeconds(_options.ExpirationCushion * -1))
                        SetCache(RefreshToken(jwtSecurityToken.RefreshToken));

                return jwtSecurityToken?.AccessToken;
            }
        }

        private void SetCache(JwtSecurityToken jwtSecurityToken)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(_options.CacheKey, jwtSecurityToken, cacheEntryOptions);
        }

        private JwtSecurityToken GetToken()
        {
            JwtTokenService tokenService = new JwtTokenService(_httpClient);

            JwtSecurityToken token = tokenService.GetToken(_options);

            return token;
        }

        private JwtSecurityToken RefreshToken(string refreshToken)
        {
            JwtTokenService tokenService = new JwtTokenService(_httpClient);

            JwtSecurityToken token = tokenService.GetRefreshToken(refreshToken, _options);

            return token;
        }
    }
}