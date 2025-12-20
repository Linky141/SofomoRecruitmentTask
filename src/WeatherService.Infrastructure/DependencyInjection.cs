using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherService.Application.Interfaces;
using WeatherService.Infrastructure.Persistence;
using WeatherService.Infrastructure.Repositories;

/// <summary>
/// Provides extension methods for registering infrastructure-layer services,
/// including database context, repositories, and external API clients.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Configures and registers the infrastructure layer services into the application's 
    /// dependency injection container. This includes the EF Core PostgreSQL database context,
    /// repository implementations for locations and weather forecasts, and the HTTP client service
    /// for fetching external weather data.
    /// </summary>
    /// <param name="services">The application's dependency injection container.</param>
    /// <param name="connectionString">The PostgreSQL database connection string used to configure EF Core.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance with infrastructure services registered.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<WeatherDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddHttpClient<IWeatherApiService, WeatherApiService>();

        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();

        return services;
    }
}