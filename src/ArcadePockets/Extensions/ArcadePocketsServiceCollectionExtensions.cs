using System;
using System.Diagnostics;
using ArcadePockets;
using ArcadePockets.DependencyInjection.Builders;
using ArcadePockets.Managers.Jwt;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ArcadePocketsServiceCollectionExtensions
    {
        public static IArcadePocketsBuilder AddJwtManager(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddTransient<IJwtManager, JwtManager>();

            return new ArcadePocketsBuilder(services);
        }
    }
}
