namespace ArcadePockets.Managers.Jwt
{
    public class JwtManagerOptions
    {
        public string TokenIssuerUri { get; set; }
        public string CacheKey { get; set; } = "arcade_pockets_jwt_access_token";
        public int EvictionCushion { get; set; }
    }
}