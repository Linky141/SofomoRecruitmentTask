using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherService.Application;

/// <summary>
/// Provides extension methods for setting up application layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers application layer dependencies into the provided IServiceCollection.
    /// Specifically, it registers MediatR handlers located in this assembly.
    /// </summary>
    /// <param name="services">The IServiceCollection to which application services will be added.</param>
    /// <returns>The same IServiceCollection instance with the application services registered.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        return services;
    }
}