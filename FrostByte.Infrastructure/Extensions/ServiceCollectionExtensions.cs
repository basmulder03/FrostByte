using FrostByte.Infrastructure.Paths;
using Microsoft.Extensions.DependencyInjection;

namespace FrostByte.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrostByteInfrastructure(this IServiceCollection services)
    {
        return services
            // Paths
            .AddSingleton<IPathService, PathService>()
        ;
    }
}
