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
    /// Registers infrastructure services such as EF Core database context,
    /// repositories, and HTTP client services into the application's dependency injection container.
    /// </summary>
    /// <param name="services">The application's dependency injection container.</param>
    /// <param name="connectionString">The database connection string used to configure EF Core.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<WeatherDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddHttpClient<IWeatherApiService, WeatherApiService>();

        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();

        return services;
    }
}