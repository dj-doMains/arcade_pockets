using System;
using ArcadePockets;
using ArcadePockets.Managers.Jwt;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ArcadePocketsJwtBuilderExtensions
    {
        public static IArcadePocketsBuilder AddInMemoryStore(this IArcadePocketsBuilder builder,
            Action<JwtManagerOptions> managerOptionsAction = null)
        {
            builder.Services.Configure(managerOptionsAction);

            return builder;
        }
    }
}