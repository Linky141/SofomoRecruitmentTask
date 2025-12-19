using WeatherService.Domain.Entities;

namespace WeatherService.Application.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages WeatherForecast entities.
/// </summary>
public interface IWeatherForecastRepository
{
    /// <summary>
    /// Adds a new weather forecast to the repository.
    /// </summary>
    /// <param name="forecast">The WeatherForecast entity to add.</param>
    /// <returns>The added WeatherForecast entity, potentially with updated properties like Id.</returns>
    Task<WeatherForecast> AddAsync(WeatherForecast forecast);

    /// <summary>
    /// Retrieves the latest weather forecast for a specific location.
    /// </summary>
    /// <param name="locationId">The ID of the location to retrieve the forecast for.</param>
    /// <returns>
    /// The latest WeatherForecast entity for the given location, or null if no forecast exists.
    /// </returns>
    Task<WeatherForecast?> GetLatestByLocationIdAsync(int locationId);
}