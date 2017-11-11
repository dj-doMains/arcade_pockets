using System;

namespace ArcadePockets.Models.Jwt
{
    public class JwtSecurityToken
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}