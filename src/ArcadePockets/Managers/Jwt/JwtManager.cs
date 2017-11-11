using System;
using ArcadePockets.Managers.Jwt;
using ArcadePockets.Models.Jwt;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ArcadePockets.Managers.Jwt
{
    public class JwtManager : IJwtManager
    {
        IMemoryCache _cache;
        JwtManagerOptions _options;

        public JwtManager(IMemoryCache cache, IOptions<JwtManagerOptions> options)
        {
            _cache = cache;
            _options = options.Value;
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
                    {
                        SetCache(jwtSecurityToken);
                    }
                }

                if (DateTime.Now > jwtSecurityToken.Expiration)
                    SetCache(RefreshToken());

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
            return new JwtSecurityToken()
            {
                AccessToken = DateTime.Now.Millisecond.ToString(),
                Expiration = DateTime.Now.AddSeconds(20 - _options.EvictionCushion)
            };
        }

        private JwtSecurityToken RefreshToken()
        {
            return new JwtSecurityToken()
            {
                AccessToken = DateTime.Now.Millisecond.ToString(),
                Expiration = DateTime.Now.AddSeconds(20 - _options.EvictionCushion)
            };
        }
    }
}