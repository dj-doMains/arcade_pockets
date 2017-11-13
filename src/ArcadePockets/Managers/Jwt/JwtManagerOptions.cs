using Microsoft.Extensions.DependencyInjection;

namespace ArcadePockets.Managers.Jwt
{
    public class JwtManagerOptions
    {
        public ArcadePocketsGrantType GrantType { get; set; }
        public string TokenIssuerUri { get; set; }
        public string CacheKey { get; set; } = "arcade_pockets_jwt_access_token";
        public int ExpirationCushion { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}