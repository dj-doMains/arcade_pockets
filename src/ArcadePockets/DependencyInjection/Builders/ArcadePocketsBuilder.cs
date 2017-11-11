using System;
using Microsoft.Extensions.DependencyInjection;

namespace ArcadePockets.DependencyInjection.Builders
{
    public class ArcadePocketsBuilder : IArcadePocketsBuilder
    {
        public ArcadePocketsBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }
}