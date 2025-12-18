using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherService.Application.Interfaces;
using WeatherService.Infrastructure.Persistence;
using WeatherService.Infrastructure.Repositories;

public static class DependencyInjection
{
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