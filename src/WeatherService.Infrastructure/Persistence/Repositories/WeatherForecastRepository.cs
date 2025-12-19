using Microsoft.EntityFrameworkCore;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Persistence;

namespace WeatherService.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for managing <see cref="WeatherForecast"/> entities
/// using Entity Framework Core as the persistence layer.
/// </summary>
public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly WeatherDbContext _context;

    /// <summary>
    /// Initializes a new instance of <see cref="WeatherForecastRepository"/>.
    /// </summary>
    /// <param name="context">The EF Core database context providing access to weather-related tables.</param>
    public WeatherForecastRepository(WeatherDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a new weather forecast entry to the database.
    /// </summary>
    /// <param name="forecast">The <see cref="WeatherForecast"/> entity to add.</param>
    /// <returns>
    /// The newly added <see cref="WeatherForecast"/> entity including its generated ID.
    /// </returns>
    public async Task<WeatherForecast> AddAsync(WeatherForecast forecast)
    {
        _context.WeatherForecasts.Add(forecast);
        await _context.SaveChangesAsync();
        return forecast;
    }

    /// <summary>
    /// Retrieves the most recent forecast for a given location.
    /// </summary>
    /// <param name="locationId">The ID of the location for which to retrieve the forecast.</param>
    /// <returns>
    /// The latest <see cref="WeatherForecast"/> associated with the location,
    /// or <c>null</c> if no forecasts exist.
    /// </returns>
    public async Task<WeatherForecast?> GetLatestByLocationIdAsync(int locationId)
    {
        return await _context.WeatherForecasts
            .Where(f => f.LocationId == locationId)
            .OrderByDescending(f => f.ForecastDate)
            .FirstOrDefaultAsync();
    }
}